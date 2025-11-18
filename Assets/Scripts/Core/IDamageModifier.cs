public interface IDamageModifier
{
  void Apply(DamageContext ctx);
}


public class DamageContext
{
  public int amount;
  public DamageType DamageType;
  public DamageSourceType SourceType;
  public BallType ballType;
  public int BaseDamage;
  public int PlayerAttack;
  public HitboxType Hitbox;
  public Enemy Enemy;
  public EffectSO<EffectContext> OnHitEffect;
  public int FinalDamage;
  public bool IsBlocked;

  private DamageContext() { } // Prevent "new" from outside

  // ---- FACTORY HELPERS ----
  public static DamageContext CreateBallDamage(
      Enemy enemy,
      int baseDamage,
      HitboxType hitbox,
      BallType ballType,
      EffectSO<EffectContext> onHitEffect = null,
      DamageType damageType = DamageType.Normal
      )
  {
    return new DamageContext
    {
      SourceType = DamageSourceType.Ball,
      Enemy = enemy,
      BaseDamage = baseDamage,
      Hitbox = hitbox,
      ballType = ballType,
      OnHitEffect = onHitEffect,
      DamageType = damageType,
    };
  }

  public static DamageContext CreateEffectDamage(
      Enemy enemy,
      int baseDamage,
      DamageType damageType = DamageType.Normal
      )
  {
    return new DamageContext
    {
      SourceType = DamageSourceType.Effect,
      Enemy = enemy,
      BaseDamage = baseDamage,
      DamageType = damageType,
    };
  }

  public static DamageContext CreateFixedDamage(
      Enemy enemy,
      int fixedDamage,
      DamageType damageType = DamageType.Normal
      )
  {
    return new DamageContext
    {
      SourceType = DamageSourceType.Fixed,
      Enemy = enemy,
      FinalDamage = fixedDamage,
      DamageType = damageType,
    };
  }

  public static DamageContext CreateObstacleDamage(
      )
  {
    return new DamageContext
    {
      SourceType = DamageSourceType.Ball,
    };
  }
}

public class PlayerDamageContext
{
  public int FinalDamage;
}

public enum DamageSourceType
{
  Ball,
  Effect,
  Fixed
}