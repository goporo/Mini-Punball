using UnityEngine;

[CreateAssetMenu(menuName = "MiniPunBall/Skill/EffBuffInvincible")]
public class EffBuffInvincible : EffectSO
{
  private int duration = 1;
  public override void Execute(IEffectContext ctx)
  {
    ctx.Player.ApplyInvincibleBuff(duration);
  }

}
