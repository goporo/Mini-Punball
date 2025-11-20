using UnityEngine;

[CreateAssetMenu(menuName = "MiniPunBall/Skill/ConHasMissile")]
public class ConHasMissile : ConditionSO
{
  public override bool Evaluate(IEffectContext ctx)
  {
    return ctx.Player.Balls.HasBall(BallType.Missile);
  }
}


