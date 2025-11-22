using UnityEngine;

[CreateAssetMenu(menuName = "MiniPunBall/Enemy/StatusBurn")]
public class StatusBurn : StatusEffectSO
{
  [SerializeField] private int damageMultiply = 2;
  private bool isSpreadable = false; // Spreadable status effects can propagate to adjacent enemies

  protected override StatusEffectBase CreateRuntimeInstance()
  {
    return new BurnEffect(damageMultiply, isSpreadable, duration, triggerChance);
  }

  public void SetSpreadable(bool spreadable)
  {
    isSpreadable = spreadable;
  }

}

public class BurnEffect : StatusEffectBase
{
  private int multiply;
  private DamageContext dmgCtx;
  private bool spreadable;

  public override StatusEffectType EffectType => StatusEffectType.Burn;

  public BurnEffect(int damageMultiply, bool spreadable, int duration, float chance) : base(duration, chance)
  {
    multiply = damageMultiply;
    this.spreadable = spreadable;
  }

  protected override void ApplyEffect(Enemy enemy)
  {
    dmgCtx = DamageContext.CreateEffectDamage(
      enemy,
      multiply,
      DamageType.Fire
    );
    Debug.Log("Can spread: " + spreadable);
    if (spreadable)
    {
      var targets = LevelContext.Instance.BoardState.GetAdjacentEnemies(enemy.CurrentCell);
      foreach (var target in targets)
      {
        if (target.StatusController.GetActiveEffects().Exists(e => e.EffectType == StatusEffectType.Burn))
          continue;
        var statusEffectConfig = CombatResolver.Instance.StatusEffectDatabase.GetConfig(StatusEffectType.Burn);
        StatusExecutor.Instance.Execute(statusEffectConfig, target);
      }
    }
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
