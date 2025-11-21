using UnityEngine;

public class PlayerAttackModifier : IDamageModifier
{
  public void Apply(DamageContext ctx)
  {
    ctx.FinalDamage = (int)(ctx.PlayerAttack * ctx.BaseDamage);
  }
}
