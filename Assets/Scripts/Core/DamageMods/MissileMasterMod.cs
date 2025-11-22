using UnityEngine;

public class MissileMasterMod : IDamageModifier
{
  private readonly float buffMultiplier = 1.5f;
  public void Apply(DamageContext ctx)
  {
    if (ctx.DamageType != DamageType.Missile)
      return;

    ctx.FinalDamage = Mathf.RoundToInt(ctx.FinalDamage * buffMultiplier);
  }
}
