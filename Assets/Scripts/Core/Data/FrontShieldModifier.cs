using UnityEngine;

[CreateAssetMenu(menuName = "MiniPunBall/DamageModifier/Front Shield")]
public class FrontShieldModifierSO : DamageModifierSO
{
  public override void Apply(DamageContext ctx)
  {
    if (ctx.Hitbox != HitboxType.Front)
      return;

    ctx.FinalDamage = 0;
    ctx.IsBlocked = true;
  }
}

