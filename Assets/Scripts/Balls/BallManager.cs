using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BallPool))]
public class BallManager : MonoBehaviour
{
    private List<BallBase> movingBalls = new(); // Balls yet to return
    private List<BallBase> listBalls = new();    // All balls for the wave
    public int RemainingBalls => listBalls.Count - movingBalls.Count;
    private BallPool ballPool;

    public static event Action<List<BallBase>> OnAllBallsReturned;

    void Awake()
    {
        ballPool = GetComponent<BallPool>();
    }

    void Start()
    {
        for (int i = 0; i < GameManager.Instance.CharacterSO.BaseBallsCount; i++)
        {
            var ball = ballPool.Get();
            ballPool.Return(ball);
        }
    }

    public void RegisterBall(BallBase ball)
    {
        ball.GetComponent<BallPhysics>().OnReturned += HandleBallReturned;

        // If this is the first ball of a new wave, reset lists
        if (movingBalls.Count == 0)
        {
            movingBalls.Clear();
            listBalls.Clear();
        }
        movingBalls.Add(ball);
        listBalls.Add(ball);
    }

    private void HandleBallReturned(BallBase ball)
    {
        ball.GetComponent<BallPhysics>().OnReturned -= HandleBallReturned;
        ballPool.Return(ball);
        movingBalls.Remove(ball);

        if (movingBalls.Count == 0)
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

