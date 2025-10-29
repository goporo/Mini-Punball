using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(PlayerRunStats))]
public class PlayerShooter : MonoBehaviour
{
    [SerializeField] private Transform shootOrigin;

    private Plane aimPlane;
    PlayerRunStats playerRunStats;
    BallManager ballManager;
    int ballCount;
    private bool canShoot = false;


    void Awake()
    {
        playerRunStats = GetComponent<PlayerRunStats>();
        ballManager = GetComponentInChildren<BallManager>();
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
    }

    void Start()
    {
        aimPlane = new Plane(Vector3.up, new Vector3(0, 1f, 0));
        ballCount = GameManager.Instance.CharacterSO.BaseBallsCount;
        BallType ballType = GameManager.Instance.CharacterSO.BallConfig.BallType;
        List<BallType> ballList = Enumerable.Repeat(ballType, ballCount).ToList();
        ballManager.Init(ballList);

    }

    void Update()
    {
        if (Input.GetMouseButtonUp(0) && canShoot)
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
            EventBus.Publish(new PlayerCanShootEvent(false));
        }
    }

    private IEnumerator ShootBallsSequentially(Vector3 dir)
    {
        Debug.Log("Shooting balls sequentially");
        float delay = 0.1f;

        // Shoot all balls in the player's collection
        while (ballManager.RemainingBalls > 0)
        {
            var ballBase = ballManager.SpawnNextBall(shootOrigin.position, Quaternion.identity);
            if (ballBase != null)
            {
                ballBase.Init(playerRunStats, dir);
                EventBus.Publish(new BallFiredEvent(ballBase));

                // Add delay between shots if there are more balls to shoot
                if (ballManager.RemainingBalls > 0)
                    yield return new WaitForSeconds(delay);
            }
            else
            {
                break; // No more balls to shoot
            }
        }
    }


}
