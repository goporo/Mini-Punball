using UnityEngine;

public class AttackModifier : IDamageModifier
{
  public void Apply(DamageContext ctx)
  {
    foreach (var modifier in ctx.AttackModifier)
    {
      modifier?.Apply(ctx);
    }
  }
}
