using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum BallBuffTarget
{
    All,
    Normal,
    Special,
    First,
    Last,
}

public struct BallBuff
{
    public float AttackMultiplier;
    public BallBuffTarget Target;

    public BallBuff(float multiplier, BallBuffTarget target)
    {
        AttackMultiplier = multiplier;
        Target = target;
    }
}

public class BallManager : MonoBehaviour
{
    private Coroutine ballStuckRoutine;
    [SerializeField] private BallDatabaseSO ballDatabaseSO;

    public List<BallBase> playerBalls = new(); // Balls player owns in order
    public int RemainingBalls => Mathf.Max(0, playerBalls.Count - ballsShot);

    // Active wave tracking
    private List<BallBase> activeBalls = new(); // Balls currently in motion during this wave
    private List<BallBase> returnedBalls = new(); // Balls that have returned this wave
    private int ballsShot = 0; // How many balls have been shot this wave


    private CharacterSO characterSO;
    private List<BallBuff> activeBallBuffs = new();
    private List<BallBase> ephemeralBalls = new();

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

    public void ApplyBallBuff(BallBuff buff)
    {
        activeBallBuffs.Add(buff);
    }

    public float GetBallAttack(BallBase ball)
    {
        float baseAttack = ball.Stats.BaseDamage;
        int ballIndex = playerBalls.IndexOf(ball);
        if (ballIndex == -1)
        {
            if (ephemeralBalls.Contains(ball))
            {
                return baseAttack * CalculateBuffMultiplier(ball, -1);
            }
            Debug.LogWarning("Ball not found in playerBalls or ephemeralBalls list, using base damage");
            return baseAttack;
        }
        return baseAttack * CalculateBuffMultiplier(ball, ballIndex);
    }

    private float CalculateBuffMultiplier(BallBase ball, int ballIndex)
    {
        float multiplier = 1f;
        foreach (var buff in activeBallBuffs)
        {
            if (buff.Target == BallBuffTarget.All ||
            (buff.Target == BallBuffTarget.First && ballIndex == 0) ||
            (buff.Target == BallBuffTarget.Last && ballIndex == playerBalls.Count - 1) ||
            (buff.Target == BallBuffTarget.Normal && ball.Stats.BallType == BallType.Normal) ||
            (buff.Target == BallBuffTarget.Special && ball.Stats.BallType != BallType.Normal))
            {
                multiplier *= buff.AttackMultiplier;
            }
        }
        return multiplier;
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

        if (ballType == BallType.Void || ballType == BallType.Bomb)
        {
            // Special case: Void ball goes to the front
            playerBalls.Insert(0, ballBase);
        }
        else
        {
            playerBalls.Add(ballBase);
        }
        EventBus.Publish(new BallCountChangedEvent(playerBalls.Count));
    }

    public void SpawnEphemeralBall(BallType type, int count, Vector3 position, Vector3 direction, Quaternion? rotation = null)
    {
        var ballConfig = ballDatabaseSO.GetConfig(type);
        for (int i = 0; i < count; i++)
        {
            var ballObj = Instantiate(ballConfig.BallPrefab, transform);
            var ballBase = ballObj.GetComponent<BallBase>();
            ballBase.transform.position = position;


            // Fire the ball immediately
            ballBase.transform.SetPositionAndRotation(position, rotation ?? Quaternion.identity);
            ballBase.Init(LevelContext.Instance.Player, direction);

            ephemeralBalls.Add(ballBase);
            activeBalls.Add(ballBase);
        }
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

        // Check if all balls (including ephemeral) have returned
        int totalBallsShot = ballsShot + ephemeralBalls.Count;
        int totalReturned = returnedBalls.Count;

        if (activeBalls.Count == 0 && totalReturned >= totalBallsShot)
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
            {
                ball.gameObject.SetActive(false);
            }
        }

        // Destroy ephemeral balls
        foreach (var ball in ephemeralBalls)
        {
            if (ball != null)
            {
                Destroy(ball.gameObject);
            }
        }
        ephemeralBalls.Clear();

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
        const int limitTime = 15;
        yield return new WaitForSeconds(limitTime);
        if (activeBalls.Count > 0)
        {
            EventBus.Publish(new BallStuckEvent(limitTime));
        }
    }


}