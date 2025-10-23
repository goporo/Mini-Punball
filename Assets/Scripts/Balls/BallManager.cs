using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BallPool))]
public class BallManager : MonoBehaviour
{
    private List<BallBase> activeBalls = new(); // Balls yet to return
    private List<BallBase> listBalls = new();    // All balls for the wave
    private BallPool ballPool;

    public static event Action<List<BallBase>> OnAllBallsReturned;

    void Awake()
    {
        ballPool = GetComponent<BallPool>();
    }

    void OnEnable()
    {
        PlayerShooter.OnBallSpawned += RegisterBall;
    }

    void OnDisable()
    {
        PlayerShooter.OnBallSpawned -= RegisterBall;
    }

    private void RegisterBall(BallBase ball)
    {
        ball.GetComponent<BallPhysics>().OnReturned += HandleBallReturned;

        // If this is the first ball of a new wave, reset lists
        if (activeBalls.Count == 0)
        {
            activeBalls.Clear();
            listBalls.Clear();
        }
        activeBalls.Add(ball);
        listBalls.Add(ball);
    }

    private void HandleBallReturned(BallBase ball)
    {
        ball.GetComponent<BallPhysics>().OnReturned -= HandleBallReturned;
        ballPool.Return(ball);
        activeBalls.Remove(ball);

        if (activeBalls.Count == 0)
        {
            OnAllBallsReturned?.Invoke(listBalls);
        }
    }

    public BallBase SpawnBall(Vector3 position, Quaternion? rotation)
    {
        BallBase ball = ballPool.Get();
        var ballPhysics = ball.GetComponent<BallPhysics>();
        if (ballPhysics != null)
            ballPhysics.ResetState();
        ball.transform.SetPositionAndRotation(position, rotation ?? Quaternion.identity);
        return ball;
    }
}

