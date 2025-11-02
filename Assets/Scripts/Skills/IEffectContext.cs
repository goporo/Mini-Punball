using UnityEngine;

public interface IEffectContext
{
  PlayerRunStats Player => GameContext.Instance.Player;
  Enemy Enemy { get; }

}

public class EffectContext : IEffectContext
{
  public Enemy Enemy { get; }
  public EffectContext(Enemy enemy)
  {
    Enemy = enemy;
  }
}

public class BallHitContext : IEffectContext
{
  public PlayerRunStats Player => GameContext.Instance.Player;
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

public struct EnemyDeathContext
{
  public GameContext GameContext => GameContext.Instance;
  public Enemy Enemy { get; }
  public Vector3 Position { get; }
  public EnemyDeathContext(Enemy enemy, Vector3 position)
  {
    Enemy = enemy;
    Position = position;

  }
}


public struct AddBallContext
{

  public BallType BallType { get; }

  public AddBallContext(BallType ballType)
  {

    BallType = ballType;
  }
}



public struct SkillSelectedContext
{

  public PlayerSkillSO SelectedSkill { get; }

  public SkillSelectedContext(PlayerSkillSO selectedSkill)
  {

    SelectedSkill = selectedSkill;
  }
}
