using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BallManager : MonoBehaviour
{
    private Coroutine ballStuckRoutine;
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
        characterSO = GlobalContext.Instance.CharacterSO;
    }

    private void OnEnable()
    {
        EventBus.Subscribe<BallFiredEvent>(OnBallFired);
        EventBus.Subscribe<BallReturnedEvent>(OnBallReturned);
        EventBus.Subscribe<PickupBallEvent>(HandlePickupBall);
        EventBus.Subscribe<AllBallShotEvent>(OnAllBallShot);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<BallFiredEvent>(OnBallFired);
        EventBus.Unsubscribe<BallReturnedEvent>(OnBallReturned);
        EventBus.Unsubscribe<PickupBallEvent>(HandlePickupBall);
        EventBus.Unsubscribe<AllBallShotEvent>(OnAllBallShot);
    }

    private void HandlePickupBall(PickupBallEvent e)
    {
        AddBall();
    }

    public void AddBall(BallType? type = null)
    {
        BallType ballType = type ?? characterSO.BaseBallConfig.BallType;
        var ballConfig = ballDatabaseSO.GetConfig(ballType);
        var ballObj = Instantiate(ballConfig.BallPrefab, transform);
        var ballBase = ballObj.GetComponent<BallBase>();
        ballObj.SetActive(false);
        playerBalls.Add(ballBase);
        EventBus.Publish(new BallCountChangedEvent(playerBalls.Count));
    }

    public bool HasBall(BallType type)
    {
        foreach (var ball in playerBalls)
        {
            if (ball.Stats.BallType == type)
                return true;
        }
        return false;
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
            HandleAllBallReturned();
        }
    }

    private void HandleAllBallReturned()
    {
        EventBus.Publish(new AllBallReturnedEvent(new List<BallBase>(returnedBalls)));
        ResetForNextWave();
        if (ballStuckRoutine != null)
        {
            StopCoroutine(ballStuckRoutine);
            ballStuckRoutine = null;
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

    public void ForceBallsReturn()
    {
        foreach (var ball in activeBalls.ToList())
        {
            ball.Physics.ForceReturn();
        }
    }


    private void OnAllBallShot(AllBallShotEvent e)
    {
        if (activeBalls.Count > 0)
        {
            ballStuckRoutine = StartCoroutine(BallStuckCoroutine());
        }
    }

    private IEnumerator BallStuckCoroutine()
    {
        const int limitTime = 20;
        yield return new WaitForSeconds(limitTime);
        if (activeBalls.Count > 0)
        {
            EventBus.Publish(new BallStuckEvent(limitTime));
        }
    }


}