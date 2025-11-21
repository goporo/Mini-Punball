using UnityEngine;

public class HitDirectionModifier : IDamageModifier
{
  // Global modifier only
  public void Apply(DamageContext ctx)
  {
    switch (ctx.Hitbox)
    {
      case HitboxType.Front:
        break;
      case HitboxType.Side:
        break;
      case HitboxType.Back:
        break;
    }
  }
}
