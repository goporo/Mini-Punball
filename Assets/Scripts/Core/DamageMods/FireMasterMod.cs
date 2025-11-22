using UnityEngine;

public class FireMasterMod : IDamageModifier
{
  private readonly float buffMultiplier = 1.5f;
  public void Apply(DamageContext ctx)
  {
    if (ctx.DamageType != DamageType.Fire)
      return;

    ctx.FinalDamage = Mathf.RoundToInt(ctx.FinalDamage * buffMultiplier);
  }
}
