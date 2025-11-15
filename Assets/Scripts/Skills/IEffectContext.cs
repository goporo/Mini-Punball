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

public struct EnemyDeathContext
{
  public LevelContext GameContext => LevelContext.Instance;
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
