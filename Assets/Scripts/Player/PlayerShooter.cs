using System;
using UnityEngine;

[RequireComponent(typeof(PlayerRunStats))]
public class PlayerShooter : MonoBehaviour
{
    public Transform shootOrigin;
    public GameObject ballPrefab;
    public static event Action<BallBase> OnBallSpawned;

    private Plane aimPlane;
    PlayerRunStats playerRunStats;



    void Awake()
    {
        playerRunStats = GetComponent<PlayerRunStats>();
    }

    void Start()
    {
        aimPlane = new Plane(Vector3.up, new Vector3(0, 1f, 0));
    }

    void Update()
    {
        if (Input.GetMouseButtonUp(0) && LevelRuntimeData.Instance.CanShoot)
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
            if (ball.TryGetComponent<BallBase>(out var ballBase))
            {
                ballBase.Init(playerRunStats, dir);
                OnBallSpawned?.Invoke(ballBase);
                LevelRuntimeData.Instance.CanShoot = false;
            }
        }
    }


}
