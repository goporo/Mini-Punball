using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PlayerRunStats))]
public class PlayerShooter : MonoBehaviour
{
    public Transform shootOrigin;
    public GameObject ballPrefab;
    public static event Action<BallBase> OnBallSpawned;

    private Plane aimPlane;
    PlayerRunStats playerRunStats;
    BallManager ballManager;


    void Awake()
    {
        playerRunStats = GetComponent<PlayerRunStats>();
        ballManager = GetComponentInChildren<BallManager>();
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

            StartCoroutine(ShootBallsSequentially(dir));
            LevelRuntimeData.Instance.CanShoot = false;
        }
    }

    private IEnumerator ShootBallsSequentially(Vector3 dir)
    {
        int ballCount = 5;
        float delay = 0.1f;

        for (int i = 0; i < ballCount; i++)
        {
            var ballBase = ballManager.SpawnBall(shootOrigin.position, Quaternion.identity);
            if (ballBase != null)
            {
                ballBase.Init(playerRunStats, dir);
                OnBallSpawned?.Invoke(ballBase);
            }
            if (i < ballCount - 1)
                yield return new WaitForSeconds(delay);
        }
    }


}
