using UnityEngine;

public class LightningMasterMod : IDamageModifier
{
  private readonly float buffMultiplier = 1.5f;
  public void Apply(DamageContext ctx)
  {
    if (ctx.DamageType != DamageType.Lightning)
      return;

    ctx.FinalDamage = Mathf.RoundToInt(ctx.FinalDamage * buffMultiplier);
  }
}
