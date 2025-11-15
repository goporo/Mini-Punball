using UnityEngine;

[CreateAssetMenu(menuName = "MiniPunBall/Skill/EffAddBall", order = 0)]
public class EffAddBall : EffectSO
{
  public BallType BallType;
  public override void Execute(IEffectContext ctx)
  {
    LevelContext.Instance.Player.Balls.AddBall(BallType);
  }

}


