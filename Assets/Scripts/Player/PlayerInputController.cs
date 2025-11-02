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

    // Setup line renderer
    lineRenderer.positionCount = 0;
    lineRenderer.startWidth = 0.05f;
    lineRenderer.endWidth = 0.05f;
    lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
    lineRenderer.startColor = Color.white;
    lineRenderer.endColor = Color.white;

    // Setup visualization materials
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
    BallType ballType = GlobalContext.Instance.CharacterSO.BallConfig.BallType;
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
      // Only prevent dragging if clicking on screen-space UI
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
      // Keep isDragging state - don't reset it
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

  /// <summary>
  /// Check if the pointer is over any SCREEN-SPACE UI element (excludes world-space UI)
  /// </summary>
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

    // Check if any of the hit UI elements are screen-space
    foreach (RaycastResult result in results)
    {
      if (result.gameObject != null)
      {
        Canvas canvas = result.gameObject.GetComponentInParent<Canvas>();
        if (canvas != null)
        {
          // Only block input if it's screen-space UI (Overlay or Camera with screen-space)
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

      // Don't clamp the mouse point - let the aim line follow freely
      Vector3 from = shootOrigin.position;
      Vector3 to = new(hitPoint.x, shootOrigin.position.y, hitPoint.z);
      Vector3 direction = (to - from).normalized;

      DrawReflectionPath(from, direction);
    }
  }

  void DrawReflectionPath(Vector3 startPos, Vector3 direction)
  {
    Vector3 ballSize = GetBallSize();

    float minCollisionDistance = 0.01f;


    List<Vector3> points = new() { startPos };
    contactPoints.Clear();
    Vector3 currentPos = startPos;
    Vector3 currentDir = direction;
    float yLevel = startPos.y;

    for (int i = 0; i < maxBounces; i++)
    {
      if (Physics.BoxCast(currentPos, ballSize * 1.0f, currentDir, out RaycastHit hit, Quaternion.identity, maxDistance, bounceMask))
      {
        float safeDistance = Mathf.Max(0, hit.distance - minCollisionDistance);
        Vector3 hitPoint = currentPos + currentDir * safeDistance;
        hitPoint.y = yLevel;

        // Clamp both line point and contact point if below baseline
        Vector3 displayPoint = hitPoint;
        if (hitPoint.z < aimConfig.baseLineZ)
        {
          displayPoint.z = aimConfig.baseLineZ;
        }

        points.Add(displayPoint);
        contactPoints.Add(displayPoint);

        Vector3 reflection = Vector3.Reflect(currentDir, hit.normal);
        reflection.y = 0;

        if (reflection.sqrMagnitude < 0.01f)
        {
          break;
        }

        currentDir = reflection.normalized;
        currentPos = hitPoint;
      }
      else
      {
        // No contact found - check if it goes below baseline
        Vector3 endPoint = currentPos + currentDir * maxDistance;
        endPoint.y = yLevel;

        if (endPoint.z < aimConfig.baseLineZ)
        {
          // Cancel the shot - don't display anything
          lineRenderer.positionCount = 0;
          contactPoints.Clear();
          return;
        }

        points.Add(endPoint);
        break;
      }
    }

    lineRenderer.positionCount = points.Count;
    lineRenderer.SetPositions(points.ToArray());
  }

  void ShootBall()
  {
    Plane plane = new(Vector3.up, shootOrigin.position);
    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

    if (plane.Raycast(ray, out float enter))
    {
      Vector3 hit = ray.GetPoint(enter);

      // Don't shoot if aim line is not displayed (cancelled)
      if (lineRenderer.positionCount == 0)
      {
        return;
      }

      // Clamp the shooting direction to baseline
      Vector3 clampedHit = hit;
      if (clampedHit.z < aimConfig.baseLineZ)
      {
        clampedHit.z = aimConfig.baseLineZ;
      }

      Vector3 dir = (clampedHit - shootOrigin.position);
      dir.y = 0;
      dir.Normalize();

      // Clear aim line
      lineRenderer.positionCount = 0;
      contactPoints.Clear();

      // Start shooting
      StartCoroutine(ShootBallsSequentially(dir));
      EventBus.Publish(new PlayerCanShootEvent(false));
    }
  }

  private IEnumerator ShootBallsSequentially(Vector3 dir)
  {
    float delay = 0.1f;

    while (ballManager.RemainingBalls > 0)
    {
      var ballBase = ballManager.SpawnNextBall(shootOrigin.position, Quaternion.identity);
      if (ballBase != null)
      {
        ballBase.Init(playerRunStats, dir);
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
