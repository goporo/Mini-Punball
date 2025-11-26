using UnityEngine;

[CreateAssetMenu(menuName = "MiniPunBall/Skill/EffUpgradeBall")]
public class EffUpgradeBall : EffectSO
{
  private int count = 2;
  public BallType BallType;
  public override void Execute(IEffectContext ctx)
  {
    ctx.Player.Balls.UpgradeBalls(BallType, count);
  }

}


