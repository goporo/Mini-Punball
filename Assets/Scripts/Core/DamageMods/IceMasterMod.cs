using UnityEngine;

public class IceMasterMod : IDamageModifier
{
  private readonly float buffMultiplier = 1.5f;
  public void Apply(DamageContext ctx)
  {
    if (ctx.DamageType != DamageType.Ice)
      return;

    ctx.FinalDamage = Mathf.RoundToInt(ctx.FinalDamage * buffMultiplier);
  }
}
