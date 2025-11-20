using UnityEngine;

public interface IEffectContext
{
  PlayerRunStats Player { get; }

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
