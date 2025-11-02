using UnityEngine;

[CreateAssetMenu(menuName = "MiniPunBall/Skill/ConHasMissile", order = 0)]
public class ConHasMissile : ConditionSO
{
  public override bool Evaluate(IEffectContext ctx)
  {
    return ctx.Player.Balls.HasBall(BallType.Missile);
  }
}


