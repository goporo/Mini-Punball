using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BallManager : MonoBehaviour
{
    [SerializeField] private BallDatabaseSO ballDatabaseSO;

    public List<BallBase> playerBalls = new(); // Balls player owns in order

    // Active wave tracking
    private List<BallBase> activeBalls = new(); // Balls currently in motion during this wave
    private List<BallBase> returnedBalls = new(); // Balls that have returned this wave
    private int ballsShot = 0; // How many balls have been shot this wave

    // Properties
    public List<BallBase> PlayerBalls => new(playerBalls);
    public int RemainingBalls => Mathf.Max(0, playerBalls.Count - ballsShot);

    private CharacterSO characterSO;


    public void Init(List<BallType> balls)
    {
        playerBalls.Clear();
        foreach (var type in balls)
        {
            var ballConfig = ballDatabaseSO.GetConfig(type);
            var ballObj = Instantiate(ballConfig.BallPrefab, transform);
            var ballBase = ballObj.GetComponent<BallBase>();
            ballObj.SetActive(false);
            playerBalls.Add(ballBase);
        }
    }

    private void Awake()
    {
        characterSO = GameManager.Instance.CharacterSO;
    }

    private void OnEnable()
    {
        EventBus.Subscribe<BallFiredEvent>(OnBallFired);
        EventBus.Subscribe<BallReturnedEvent>(OnBallReturned);
        EventBus.Subscribe<PickupBallEvent>(HandlePickupBall);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<BallFiredEvent>(OnBallFired);
        EventBus.Unsubscribe<BallReturnedEvent>(OnBallReturned);
        EventBus.Unsubscribe<PickupBallEvent>(HandlePickupBall);
    }

    private void HandlePickupBall(PickupBallEvent e)
    {
        for (int i = 0; i < e.Amount; i++)
        {
            AddBall();
        }
    }

    private void AddBall(BallType? type = null)
    {
        BallType ballType = type ?? characterSO.BallConfig.BallType;
        var ballConfig = ballDatabaseSO.GetConfig(ballType);
        var ballObj = Instantiate(ballConfig.BallPrefab, transform);
        var ballBase = ballObj.GetComponent<BallBase>();
        ballObj.SetActive(false);
        playerBalls.Add(ballBase);
        EventBus.Publish(new BallCountChangedEvent(playerBalls.Count));
    }

    private void OnBallFired(BallFiredEvent e)
    {
        var ball = e.BallBase;
        activeBalls.Add(ball);
        EventBus.Publish(new BallCountChangedEvent(RemainingBalls));
    }

    private void OnBallReturned(BallReturnedEvent e)
    {
        var ball = e.BallBase;

        activeBalls.Remove(ball);
        returnedBalls.Add(ball);

        ball.gameObject.SetActive(false);

        if (activeBalls.Count == 0)
        {
            EventBus.Publish(new AllBallReturnedEvent(new List<BallBase>(returnedBalls)));
            ResetForNextWave();
        }
    }

    private void ResetForNextWave()
    {
        ballsShot = 0;
        foreach (var ball in returnedBalls)
        {
            if (ball != null)
                ball.gameObject.SetActive(false);
        }
        activeBalls.Clear();
        returnedBalls.Clear();
    }

    public BallBase SpawnNextBall(Vector3 position, Quaternion? rotation = null)
    {
        var ballBase = playerBalls[ballsShot];
        ballsShot++;
        if (ballsShot == playerBalls.Count)
        {
            EventBus.Publish(new AllBallShotEvent());
        }
        ballBase.transform.SetPositionAndRotation(position, rotation ?? Quaternion.identity);
        ballBase.gameObject.SetActive(true);
        // Optionally reset state if needed
        var ballPhysics = ballBase.GetComponent<BallPhysics>();
        ballPhysics?.ResetState();
        return ballBase;
    }

}