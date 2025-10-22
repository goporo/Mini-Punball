using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(LineRenderer))]
public class AimLine : MonoBehaviour
{
    public Transform shootOrigin;
    public int maxBounces = 5;
    public float maxDistance = 50f;
    public LayerMask bounceMask;
    public GameObject ballPrefab;

    private LineRenderer lineRenderer;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 0;
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.white;
        lineRenderer.endColor = Color.white;
    }

    Vector3 GetBallSize()
    {
        if (ballPrefab != null)
        {
            var ballBehaviour = ballPrefab.GetComponent<BallPhysics>();
            if (ballBehaviour != null)
            {
                return ballBehaviour.BoxSize;
            }
        }
        return Vector3.one * 0.4f; // Fallback
    }

    void Update()
    {
        if (Input.GetMouseButton(0) && LevelRuntimeData.Instance.CanShoot)
        {
            Plane xzPlane = new Plane(Vector3.up, shootOrigin.position);
            Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (xzPlane.Raycast(mouseRay, out float distance))
            {
                Vector3 hitPoint = mouseRay.GetPoint(distance);
                Vector3 from = shootOrigin.position;
                Vector3 to = hitPoint;
                from.y = shootOrigin.position.y;
                to.y = shootOrigin.position.y;
                Vector3 direction = (to - from).normalized;

                DrawReflectionPath(from, direction);
            }
        }
        else
        {
            lineRenderer.positionCount = 0;
        }
    }
    void DrawReflectionPath(Vector3 startPos, Vector3 direction)
    {
        Vector3 ballSize = GetBallSize();
        List<Vector3> points = new List<Vector3> { startPos };
        Vector3 currentPos = startPos;
        Vector3 currentDir = direction;
        float yLevel = startPos.y;

        for (int i = 0; i < maxBounces; i++)
        {
            if (Physics.BoxCast(currentPos, ballSize * 1f, currentDir, out RaycastHit hit, Quaternion.identity, maxDistance, bounceMask))
            {
                Vector3 hitPoint = currentPos + currentDir * hit.distance;
                hitPoint.y = yLevel;
                points.Add(hitPoint);

                currentDir = Vector3.Reflect(currentDir, hit.normal);
                currentDir.y = 0;
                currentDir = currentDir.normalized;

                currentPos = hitPoint + currentDir * 0.01f;
            }
            else
            {
                Vector3 endPoint = currentPos + currentDir * maxDistance;
                endPoint.y = yLevel;
                points.Add(endPoint);
                break;
            }
        }

        lineRenderer.positionCount = points.Count;
        lineRenderer.SetPositions(points.ToArray());
    }
}
