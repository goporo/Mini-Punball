
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

public struct PickupHealthEvent : IGameEvent
{
  public int Amount;
  public PickupHealthEvent(int amount) => Amount = amount;
}

public struct OnCollectibleSpawnEvent : IGameEvent
{
  public Collectible collectible;
  public OnCollectibleSpawnEvent(Collectible collectible) => this.collectible = collectible;
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
  public Enemy Enemy => Context.Enemy;
  public BallHitContext Context;
  public EnemyDeathEvent(BallHitContext context) => Context = context;
}

public struct OnWaveStartEvent : IGameEvent
{
  public int LevelNumber;
  public int WaveNumber;
  public Enemy[] AvailableEnemies;
  public string WaveText;

  public OnWaveStartEvent(int levelNumber, int waveNumber, string waveText, Enemy[] availableEnemies = null)
  {
    LevelNumber = levelNumber;
    WaveNumber = waveNumber;
    WaveText = waveText;
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

public struct BallStuckEvent : IGameEvent
{
  public int ElapsedSeconds;
  public BallStuckEvent(int elapsedSeconds) => ElapsedSeconds = elapsedSeconds;
}

public struct OnBossWaveApproachingEvent : IGameEvent
{
}

public struct OnBossWaveStartEvent : IGameEvent
{
  public Enemy BossEnemy;
  public OnBossWaveStartEvent(Enemy bossEnemy) => BossEnemy = bossEnemy;
}

public struct LevelCompleteEvent : IGameEvent
{
  public LevelSO LevelData;
  public LevelResult Result;
  public LevelCompleteEvent(LevelSO levelData, LevelResult result)
  {
    LevelData = levelData;
    Result = result;
  }
}

public struct OnPlayerDiedEvent : IGameEvent
{
}