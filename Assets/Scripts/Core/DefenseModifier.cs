using UnityEngine;

public class DefenseModifier : IDamageModifier
{
  public void Apply(DamageContext ctx)
  {
    foreach (var modifier in ctx.DefenseModifiers)
    {
      modifier?.Apply(ctx);
    }
  }
}
