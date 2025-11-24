using System.Collections.Generic;
using UnityEngine;

public interface IEffectContext
{
  PlayerRunStats Player { get; }

}

public class PlayerContext : IEffectContext
{
  public PlayerRunStats Player { get; }
  public PlayerContext(PlayerRunStats player)
  {
    Player = player;
  }
}

public class EffectContext : IEffectContext
{
  public PlayerRunStats Player => LevelContext.Instance.Player;
  public Enemy Enemy { get; }
  public EffectContext(Enemy enemy)
  {
    Enemy = enemy;
  }
}

public class SkillGainContext : IEffectContext
{
  public PlayerRunStats Player => LevelContext.Instance.Player;
  public SkillRuntime SkillRuntime { get; }
  public SkillGainContext(SkillRuntime skillRuntime)
  {
    SkillRuntime = skillRuntime;
  }
}

public class EffectCastContext : IEffectContext
{
  public PlayerRunStats Player => LevelContext.Instance.Player;
  public Enemy CastEnemy;
  public ECastSource CastSource;
  public EffectCastContext(Enemy castEnemy = null, ECastSource castOrigin = ECastSource.Enemy)
  {
    this.CastEnemy = castEnemy;
    this.CastSource = castOrigin;
  }
}

public class ComboCastContext : IEffectContext
{
  public PlayerRunStats Player => LevelContext.Instance.Player;
  public EffectSO<EffectCastContext> Effect;
  public ECastSource CastOrigin => ECastSource.Combo;
  public ComboCastContext(EffectSO<EffectCastContext> effect)
  {
    this.Effect = effect;
  }
}

public class WaveContext : IEffectContext
{
  public PlayerRunStats Player => LevelContext.Instance.Player;
  public int WaveNumber;
  public WaveContext(int waveNumber)
  {
    WaveNumber = waveNumber;
  }
}

public class AOEContext : IEffectContext
{
  public PlayerRunStats Player => LevelContext.Instance.Player;
  public List<Enemy> Targets;
  public AOEContext(List<Enemy> targets)
  {
    Targets = targets;
  }
}

public class BallHitContext : IEffectContext
{
  public PlayerRunStats Player => LevelContext.Instance.Player;
  public Enemy Enemy { get; }
  public int Damage;
  public BallBase Ball;


  public BallHitContext(Enemy enemy, int damage, BallBase ball)
  {

    Enemy = enemy;
    Damage = damage;
    Ball = ball;
  }
}

public class EnemyDeathContext
{
  public Enemy Enemy { get; }
  // to do add source
  public EnemyDeathContext(Enemy enemy)
  {
    Enemy = enemy;
  }
}
