public struct HitResult
{
  public int finalDamage;
  public bool killed;
}

public class ResolveBallHitContext : IEffectContext
{
  public PlayerRunStats Player => LevelContext.Instance.Player;
  public Enemy Enemy;
  public int BallDamage;
  public EffectSO<EffectContext> OnHitEffect;
  public IStatusEffect StatusEffect;
  public DamageType DamageType;
  public HitboxType HitboxType;

  public ResolveBallHitContext(Enemy enemy, int ballDamage, EffectSO<EffectContext> onHitEffect, DamageType damageType)
  {
    Enemy = enemy;
    BallDamage = ballDamage;
    OnHitEffect = onHitEffect;
    DamageType = damageType;
  }
  // Add more fields as needed
}


public class ResolveEffectHitContext : IEffectContext
{
  public PlayerRunStats Player => LevelContext.Instance.Player;
  public Enemy Enemy;
  public int Damage;
  public IStatusEffect StatusEffect;
  public DamageType DamageType;

  public ResolveEffectHitContext(Enemy enemy, int baseDamage, IStatusEffect statusEffect, DamageType damageType)
  {
    Enemy = enemy;
    Damage = baseDamage;
    StatusEffect = statusEffect;
    DamageType = damageType;
  }
}

public enum DamageType
{
  Normal,
  Heal,
  Fire,
  Ice,
  Lightning,
  Laser,
  Missile,
  Explosion,
  Void,
  Split,
  Drill
}

