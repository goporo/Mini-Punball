using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallManager : MonoBehaviour
{
    private readonly List<BallBase> activeBalls = new();

    public event Action OnAllBallsReturned;

    void OnEnable()
    {
        PlayerShooter.OnBallSpawned += RegisterBall;
    }

    void OnDisable()
    {
        PlayerShooter.OnBallSpawned -= RegisterBall;
    }

    public void RegisterBall(BallBase ball)
    {
        activeBalls.Add(ball);
        ball.GetComponent<BallPhysics>().OnReturned += HandleBallReturned;
    }

    private void HandleBallReturned(BallBase ball)
    {
        ball.GetComponent<BallPhysics>().OnReturned -= HandleBallReturned;
        activeBalls.Remove(ball);

        if (activeBalls.Count == 0)
            OnAllBallsReturned?.Invoke();
    }
}

