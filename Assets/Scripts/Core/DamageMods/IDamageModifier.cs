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
  public float BaseDamage;
  public int PlayerAttack;
  public HitboxType Hitbox;
  public Enemy Enemy;
  public EffectSO<EffectCastContext> OnHitEffect;
  public StatusEffectType StatusEffect;
  public DamageModifierSO[] AttackModifier;
  public DamageModifierSO[] DefenseModifiers;
  public int FinalDamage;
  public bool IsBlocked;

  private DamageContext() { } // Prevent "new" from outside

  // ---- FACTORY HELPERS ----
  public static DamageContext CreateBallDamage(
      Enemy enemy,
      float baseDamage,
      HitboxType hitbox,
      BallType ballType,
      DamageModifierSO[] attackModifier,
      EffectSO<EffectCastContext> onHitEffect = null,
      StatusEffectType statusEffect = StatusEffectType.None,
      DamageType damageType = DamageType.Normal
      )
  {
    return new DamageContext
    {
      PlayerAttack = LevelContext.Instance.Player.CurrentAttack,
      SourceType = DamageSourceType.Ball,
      Enemy = enemy,
      BaseDamage = baseDamage,
      Hitbox = hitbox,
      ballType = ballType,
      OnHitEffect = onHitEffect,
      StatusEffect = statusEffect,
      DamageType = damageType,
      AttackModifier = attackModifier,
    };
  }

  public static DamageContext CreateEffectDamage(
      Enemy enemy,
      int baseDamage,
      DamageType damageType = DamageType.Normal,
      StatusEffectType statusEffect = StatusEffectType.None
      )
  {
    return new DamageContext
    {
      PlayerAttack = LevelContext.Instance.Player.CurrentAttack,
      SourceType = DamageSourceType.Effect,
      Enemy = enemy,
      BaseDamage = baseDamage,
      DamageType = damageType,
      StatusEffect = statusEffect,
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

  public static DamageContext CreateObstacleDamage()
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
