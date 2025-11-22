using UnityEngine;

public class HitDistanceModifier : IDamageModifier
{
  // Global modifier only
  public void Apply(DamageContext ctx)
  {
    int distance = ctx.Enemy.CurrentCell.y;

  }
}
