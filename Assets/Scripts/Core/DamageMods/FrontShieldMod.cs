using UnityEngine;

[CreateAssetMenu(menuName = "MiniPunBall/DamageModifier/Front Shield")]
public class FrontShieldMod : DamageModifierSO
{
  public override void Apply(DamageContext ctx)
  {
    if (ctx.Hitbox != HitboxType.Front)
      return;

    ctx.FinalDamage = 0;
    ctx.IsBlocked = true;
  }
}

