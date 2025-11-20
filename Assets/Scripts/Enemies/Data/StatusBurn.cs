using UnityEngine;

[CreateAssetMenu(menuName = "MiniPunBall/Enemy/StatusBurn")]
public class StatusBurn : StatusEffectSO
{
  [SerializeField] private int damageMultiply = 2;

  protected override IStatusEffect CreateRuntimeInstance()
  {
    return new BurnEffect(damageMultiply, duration, triggerChance);
  }
}

public class BurnEffect : StatusEffectBase
{
  private int multiply;
  private DamageContext dmgCtx;

  public override StatusEffectType EffectType => StatusEffectType.Burn;

  public BurnEffect(int damageMultiply, int rounds, float chance) : base(rounds, chance)
  {
    multiply = damageMultiply;
  }

  protected override void ApplyEffect(Enemy enemy)
  {
    dmgCtx = DamageContext.CreateEffectDamage(
      enemy,
      multiply,
      DamageType.Fire
    );
    enemy.EnemyStatusVisuals.SetBurning(true);
  }

  protected override void OnRoundEffect(Enemy enemy)
  {
    CombatResolver.Instance.ResolveHit(dmgCtx);
  }

  protected override void ExpireEffect(Enemy enemy)
  {
    enemy.EnemyStatusVisuals.SetBurning(false);
  }
}
