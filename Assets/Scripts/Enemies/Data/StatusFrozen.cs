using UnityEngine;

[CreateAssetMenu(menuName = "MiniPunBall/Enemy/StatusFrozen")]
public class StatusFrozen : StatusEffectSO
{
  protected override IStatusEffect CreateRuntimeInstance()
  {
    return new FrozenEffect(duration, triggerChance);
  }
}

public class FrozenEffect : StatusEffectBase
{
  public override StatusEffectType EffectType => StatusEffectType.Frozen;

  public FrozenEffect(int duration, float chance) : base(duration, chance)
  {
  }

  protected override void ApplyEffect(Enemy enemy)
  {
    enemy.EnemyStatusVisuals.SetFrozen(true);
    enemy.CanAct = false;
  }

  protected override void ExpireEffect(Enemy enemy)
  {
    enemy.EnemyStatusVisuals.SetFrozen(false);
    enemy.CanAct = true;
  }
}
