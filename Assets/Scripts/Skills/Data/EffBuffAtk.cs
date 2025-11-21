using UnityEngine;

[CreateAssetMenu(menuName = "MiniPunBall/Skill/EffBuffAtk")]
public class EffBuffAtk : EffectSO
{
  private readonly float buffMultiplier = 1.1f;
  public override void Execute(IEffectContext ctx)
  {
    ctx.Player.ApplyAttackBuff(buffMultiplier);
  }

}


