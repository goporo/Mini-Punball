using UnityEngine;

[CreateAssetMenu(menuName = "MiniPunBall/Skill/EffAddBall")]
public class EffAddBall : EffectSO
{
  public BallType BallType;
  public override void Execute(IEffectContext ctx)
  {
    ctx.Player.Balls.AddBall(BallType);
  }

}


