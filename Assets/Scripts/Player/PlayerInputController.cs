using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Centralized input controller for aiming and shooting.
/// Handles both aim line visualization and shooting logic together.
/// </summary>
[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(PlayerRunStats))]
public class PlayerInputController : MonoBehaviour
{
  [Header("Shooting Settings")]
  [SerializeField] private Transform shootOrigin;

  [Header("Aim Line Settings")]
  [SerializeField] private int maxBounces = 5;
  [SerializeField] private float maxDistance = 50f;
  [SerializeField] private LayerMask bounceMask;
  [SerializeField] private AimLineConfig aimConfig = new();

  private LineRenderer lineRenderer;
  private PlayerRunStats playerRunStats;
  private BallManager ballManager;
  private bool canShoot = false;
  private bool isDragging = false;

  private List<Vector3> contactPoints = new();
  private static Material _whiteMat;
  private static Mesh _sphereMesh;

  void Awake()
  {
    lineRenderer = GetComponent<LineRenderer>();
    playerRunStats = GetComponent<PlayerRunStats>();
    ballManager = GetComponentInChildren<BallManager>();

    lineRenderer.positionCount = 0;
    lineRenderer.startWidth = 0.05f;
    lineRenderer.endWidth = 0.05f;
    lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
    lineRenderer.startColor = Color.white;
    lineRenderer.endColor = Color.white;

    if (_whiteMat == null)
    {
      Shader shader = Shader.Find("Unlit/Color");
      if (shader == null) shader = Shader.Find("Sprites/Default");
      _whiteMat = new Material(shader)
      {
        color = Color.white
      };
    }
    if (_sphereMesh == null)
    {
      GameObject temp = GameObject.CreatePrimitive(PrimitiveType.Sphere);
      _sphereMesh = temp.GetComponent<MeshFilter>().sharedMesh;
      Destroy(temp);
    }
  }

  void Start()
  {
    int ballCount = GlobalContext.Instance.CharacterSO.BaseBallsCount;
    BallType ballType = GlobalContext.Instance.CharacterSO.BaseBallConfig.BallType;
    List<BallType> ballList = Enumerable.Repeat(ballType, ballCount).ToList();
    ballManager.Init(ballList);
  }

  void OnEnable()
  {
    EventBus.Subscribe<PlayerCanShootEvent>(SetCanShoot);
  }

  void OnDisable()
  {
    EventBus.Unsubscribe<PlayerCanShootEvent>(SetCanShoot);
  }

  private void SetCanShoot(PlayerCanShootEvent evt)
  {
    canShoot = evt.CanShoot;
    if (!canShoot)
    {
      lineRenderer.positionCount = 0;
      contactPoints.Clear();
      isDragging = false;
    }
  }

  Vector3 GetBallSize()
  {
    return Vector3.one * 0.25f;
  }

  void Update()
  {
    if (Input.GetMouseButtonDown(0))
    {
      if (IsPointerOverUI())
      {
        isDragging = false;
        return;
      }
      isDragging = true;
    }

    if (!canShoot)
    {
      lineRenderer.positionCount = 0;
      contactPoints.Clear();
      return;
    }

    if (Input.GetMouseButton(0) && isDragging)
    {
      UpdateAimLine();
    }

    if (Input.GetMouseButtonUp(0) && isDragging)
    {
      isDragging = false;
      ShootBall();
    }
  }

  private bool IsPointerOverUI()
  {
    if (EventSystem.current == null)
      return false;

    PointerEventData pointerData = new PointerEventData(EventSystem.current)
    {
      position = Input.mousePosition
    };

    List<RaycastResult> results = new List<RaycastResult>();
    EventSystem.current.RaycastAll(pointerData, results);

    foreach (RaycastResult result in results)
    {
      if (result.gameObject != null)
      {
        Canvas canvas = result.gameObject.GetComponentInParent<Canvas>();
        if (canvas != null)
        {
          if (canvas.renderMode == RenderMode.ScreenSpaceOverlay ||
              canvas.renderMode == RenderMode.ScreenSpaceCamera)
          {
            return true;
          }
          // World-space canvas should NOT block shooting
        }
      }
    }

    // Additional check for touch input on mobile
    if (Input.touchCount > 0)
    {
      pointerData.position = Input.GetTouch(0).position;
      results.Clear();
      EventSystem.current.RaycastAll(pointerData, results);

      foreach (RaycastResult result in results)
      {
        if (result.gameObject != null)
        {
          Canvas canvas = result.gameObject.GetComponentInParent<Canvas>();
          if (canvas != null &&
              (canvas.renderMode == RenderMode.ScreenSpaceOverlay ||
               canvas.renderMode == RenderMode.ScreenSpaceCamera))
          {
            return true;
          }
        }
      }
    }

    return false;
  }

  void UpdateAimLine()
  {
    Plane xzPlane = new(Vector3.up, shootOrigin.position);
    Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);

    if (xzPlane.Raycast(mouseRay, out float distance))
    {
      Vector3 hitPoint = mouseRay.GetPoint(distance);

      // Cancel and clear aim line if below cancel line
      if (hitPoint.z < aimConfig.CancelLineZ)
      {
        lineRenderer.positionCount = 0;
        contactPoints.Clear();
        return;
      }

      // Don't clamp the mouse point - let the aim line follow freely
      Vector3 from = shootOrigin.position;
      Vector3 to = new(hitPoint.x, shootOrigin.position.y, hitPoint.z);
      Vector3 direction = (to - from).normalized;

      DrawReflectionPath(from, direction);
    }
  }

  void DrawReflectionPath(Vector3 startPos, Vector3 rawDir)
  {
    // ---------- 1. PREPARE DATA ----------
    Vector3 ballSize = GetBallSize();
    float minCollisionDistance = 0.01f;
    float yLevel = startPos.y;

    // Compute angle clamp boundaries
    Vector3 leftDir = aimConfig.LeftMostPoint - startPos;
    Vector3 rightDir = aimConfig.RightMostPoint - startPos;

    leftDir.y = 0; leftDir.Normalize();
    rightDir.y = 0; rightDir.Normalize();

    // Clamp user direction into angle cone
    rawDir.y = 0;
    Vector3 shootDir = ClampToCone(rawDir.normalized, leftDir, rightDir);

    // Storage for final display points
    List<Vector3> points = new() { startPos };
    contactPoints.Clear();

    Vector3 currentPos = startPos;
    Vector3 currentDir = shootDir;


    // ---------- 2. BOUNCE LOOP ----------
    for (int i = 0; i < maxBounces; i++)
    {
      if (Physics.BoxCast(
          currentPos,
          ballSize,
          currentDir,
          out RaycastHit hit,
          Quaternion.identity,
          maxDistance,
          bounceMask))
      {
        // Ensure no overlap with collider
        float safeDist = Mathf.Max(hit.distance - minCollisionDistance, 0f);

        // Compute hit point
        Vector3 hitPoint = currentPos + currentDir * safeDist;
        hitPoint.y = yLevel; // keep flat on XZ

        // Store for line rendering
        points.Add(hitPoint);
        contactPoints.Add(hitPoint);

        // Compute reflection direction
        Vector3 reflect = Vector3.Reflect(currentDir, hit.normal);
        reflect.y = 0;

        // If reflection too tiny, stop
        if (reflect.sqrMagnitude < 0.01f)
          break;

        // Continue tracing
        currentDir = reflect.normalized;
        currentPos = hitPoint;
      }
      else
      {
        // No hit: draw until max range
        Vector3 endPoint = currentPos + currentDir * maxDistance;
        endPoint.y = yLevel;

        points.Add(endPoint);
        break;
      }
    }



    // ---------- 3. RENDER ----------

    lineRenderer.positionCount = points.Count;
    lineRenderer.SetPositions(points.ToArray());
  }

  Vector3 ClampToCone(Vector3 dir, Vector3 leftDir, Vector3 rightDir)
  {
    // dir should stay between leftDir and rightDir
    float leftCheck = Vector3.SignedAngle(leftDir, dir, Vector3.up);
    float rightCheck = Vector3.SignedAngle(dir, rightDir, Vector3.up);

    if (leftCheck < 0)
      return leftDir;

    if (rightCheck < 0)
      return rightDir;

    return dir;
  }


  void ShootBall()
  {
    Plane plane = new(Vector3.up, shootOrigin.position);
    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

    if (!plane.Raycast(ray, out float enter))
      return;

    Vector3 hit = ray.GetPoint(enter);

    // Cancel if below cancel line Z
    if (hit.z < aimConfig.CancelLineZ)
      return;

    // If line renderer is invisible → invalid aim
    if (lineRenderer.positionCount == 0)
      return;

    // ----------- ANGLE CLAMP -----------
    Vector3 rawDir = hit - shootOrigin.position;
    rawDir.y = 0;
    rawDir.Normalize();

    // Boundaries
    Vector3 leftDir = (aimConfig.LeftMostPoint - shootOrigin.position);
    Vector3 rightDir = (aimConfig.RightMostPoint - shootOrigin.position);

    leftDir.y = 0; leftDir.Normalize();
    rightDir.y = 0; rightDir.Normalize();

    Vector3 clampedDir = ClampToCone(rawDir, leftDir, rightDir);

    // ----------- CLEAR VISUALS -----------
    lineRenderer.positionCount = 0;
    contactPoints.Clear();

    // ----------- FIRE BALLS -----------
    StartCoroutine(ShootBallsSequentially(clampedDir, rotation: Quaternion.LookRotation(clampedDir)));
    EventBus.Publish(new PlayerCanShootEvent(false));
  }


  private IEnumerator ShootBallsSequentially(Vector3 dir, Quaternion rotation = default)
  {
    float delay = 0.1f;

    while (ballManager.RemainingBalls > 0)
    {
      var ballBase = ballManager.SpawnNextBall(shootOrigin.position);
      if (ballBase != null)
      {
        ballBase.Init(playerRunStats, shootOrigin.position, dir, rotation);
        EventBus.Publish(new BallFiredEvent(ballBase));

        if (ballManager.RemainingBalls > 0)
          yield return new WaitForSeconds(delay);
      }
      else
      {
        break;
      }
    }
  }

  // Visualize contact point with bigger sphere
  void OnRenderObject()
  {
    if (_whiteMat == null || _sphereMesh == null || contactPoints.Count == 0) return;
    _whiteMat.SetPass(0);
    Vector3 ballSize = GetBallSize();
    Vector3 visualSize = ballSize * 2f;

    foreach (var pos in contactPoints)
    {
      Matrix4x4 matrix = Matrix4x4.TRS(pos, Quaternion.identity, visualSize);
      Graphics.DrawMeshNow(_sphereMesh, matrix);
    }
  }
}
