using UnityEngine;

[CreateAssetMenu(menuName = "MiniPunBall/Enemy/StatusFrozen")]
public class StatusFrozen : StatusEffectSO
{
  protected override StatusEffectBase CreateRuntimeInstance()
  {
    return new FrozenEffect(duration, triggerChance);
  }

  public void IncreaseChance(float multiplier)
  {
    triggerChance *= multiplier;
    if (triggerChance > 1f)
      triggerChance = 1f;
  }

  public void IncreaseDuration(int extraTurns)
  {
    duration += extraTurns;
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
