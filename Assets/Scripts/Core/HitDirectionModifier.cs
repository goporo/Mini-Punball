using UnityEngine;

public class HitDirectionModifier : IDamageModifier
{
  public void Apply(DamageContext ctx)
  {
    // Example: back hit = bonus damage
    switch (ctx.Hitbox)
    {
      case HitboxType.Front:
        if (ctx.Enemy.Data.Specie == EnemySpecie.Shielder)
        {
          ctx.FinalDamage = 0;
          ctx.IsBlocked = true;
        }
        break;
      case HitboxType.Side:
        break;
      case HitboxType.Back:
        if (ctx.ballType == BallType.Stab)
        {
          ctx.FinalDamage = Mathf.RoundToInt(ctx.FinalDamage * 1.5f);
        }
        break;
    }
  }
}
