
using System.Collections.Generic;


/// <summary>
/// Marker interface for all events.
/// </summary>
public interface IGameEvent { }

[DontLogEvent]
public struct BallReturnedEvent : IGameEvent
{
  public BallBase BallBase;
  public BallReturnedEvent(BallBase ballBase) => BallBase = ballBase;
}

[DontLogEvent]
public struct BallFiredEvent : IGameEvent
{
  public BallBase BallBase;
  public BallFiredEvent(BallBase ballBase) => BallBase = ballBase;
}

[DontLogEvent]
public struct BallCountChangedEvent : IGameEvent
{
  public int CurrentBallCount;
  public BallCountChangedEvent(int currentBallCount) => CurrentBallCount = currentBallCount;
}

public struct PickupBallEvent : IGameEvent
{
}
public struct PickupBoxEvent : IGameEvent
{
}

public struct PickupCollectedEvent : IGameEvent
{
  public IPickupable Pickup;
  public PickupCollectedEvent(IPickupable pickup) => Pickup = pickup;
}

[DontLogEvent]
public struct BoardObjectDeathEvent : IGameEvent
{
  public BoardObject BoardObject;
  public BoardObjectDeathEvent(BoardObject boardObject) => BoardObject = boardObject;
}

public struct AllBallShotEvent : IGameEvent
{
}

public struct AllBallReturnedEvent : IGameEvent
{
  public List<BallBase> ReturnedBalls;
  public AllBallReturnedEvent(List<BallBase> returnedBalls) => ReturnedBalls = returnedBalls;
}

public struct PlayerCanShootEvent : IGameEvent
{
  public bool CanShoot;
  public PlayerCanShootEvent(bool canShoot) => CanShoot = canShoot;
}

public struct SkillSelectedEvent : IGameEvent
{
  public IEffectContext Context;
  public PlayerSkillSO PlayerSkillSO;

  public SkillSelectedEvent(IEffectContext context, PlayerSkillSO playerSkillSO)
  {
    Context = context;
    PlayerSkillSO = playerSkillSO;
  }
}

public struct SkillSelectionCompleteEvent : IGameEvent
{
}

public struct EnemyDeathEvent : IGameEvent
{
  public BallHitContext Context;
  public EnemyDeathEvent(BallHitContext context) => Context = context;
}

public struct OnWaveStartEvent : IGameEvent
{
  public int LevelNumber;
  public int WaveNumber;
  public Enemy[] AvailableEnemies;

  public OnWaveStartEvent(int levelNumber, int waveNumber, Enemy[] availableEnemies = null)
  {
    LevelNumber = levelNumber;
    WaveNumber = waveNumber;
    AvailableEnemies = availableEnemies;
  }
}

public struct OnSkillPickedEvent : IGameEvent
{
  public PlayerSkillSO SkillData;

  public OnSkillPickedEvent(PlayerSkillSO skillData)
  {
    SkillData = skillData;
  }
}

public struct OnHitEvent : IGameEvent
{
  public PlayerRunStats player;
  public Enemy enemy;
  public int damageDealt;
  public bool killed;
  public DamageType damageType;

  public OnHitEvent(
      PlayerRunStats player,
      Enemy enemy,
      int damageDealt,
      bool killed,
      DamageType damageType)
  {
    this.player = player;
    this.enemy = enemy;
    this.damageDealt = damageDealt;
    this.killed = killed;
    this.damageType = damageType;
  }
}