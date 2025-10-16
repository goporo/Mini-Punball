using UnityEngine;

public class ShooterController : MonoBehaviour
{
    public Transform shootOrigin;
    public GameObject ballPrefab;
    public float shootForce = 15f;
    public float fixedY = 1f;

    private Plane aimPlane;

    void Start()
    {
        aimPlane = new Plane(Vector3.up, new Vector3(0, fixedY, 0));
    }

    void Update()
    {
        if (Input.GetMouseButtonUp(0) && GameContext.Instance.CanShoot)
        {
            ShootBall();
        }
    }

    void ShootBall()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (aimPlane.Raycast(ray, out float enter))
        {
            Vector3 hit = ray.GetPoint(enter);
            Vector3 dir = hit - shootOrigin.position;
            dir.y = 0;
            dir.Normalize();

            GameObject ball = Instantiate(ballPrefab, shootOrigin.position, Quaternion.identity);
            var ballBehaviour = ball.GetComponent<BallBehaviour>();
            if (ballBehaviour != null)
            {
                ballBehaviour.SetDirection(dir);
                GameContext.Instance.CanShoot = false;
            }
        }
    }

}
