using UnityEngine;

[CreateAssetMenu(menuName = "MiniPunBall/Skill/EffCloneBall")]
public class EffCloneBall : EffectSO<EffectCastContext>
{
  public override void Execute(EffectCastContext ctx)
  {
    ctx.Player.Balls.CloneAttackingBall();
  }

}