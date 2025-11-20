using UnityEngine;

public abstract class StatusEffectBase : IStatusEffect
{
  protected int duration;
  protected float triggerChance;
  protected bool isActive;

  public abstract StatusEffectType EffectType { get; }

  protected StatusEffectBase(int duration, float triggerChance = 1f)
  {
    this.duration = duration;
    this.triggerChance = triggerChance;
  }

  public virtual void OnApply(Enemy enemy)
  {
    isActive = TryTrigger();
    if (!isActive) return;
    ApplyEffect(enemy);
  }

  public virtual void OnRound(Enemy enemy)
  {
    if (!isActive) return;
    OnRoundEffect(enemy);
    duration--;
  }

  public virtual void OnExpire(Enemy enemy)
  {
    if (!isActive) return;
    ExpireEffect(enemy);
  }

  protected virtual bool TryTrigger()
  {
    if (triggerChance >= 1f) return true;
    float roll = Random.Range(0f, 1f);
    return roll < triggerChance;
  }

  protected abstract void ApplyEffect(Enemy enemy);
  protected virtual void OnRoundEffect(Enemy enemy) { }
  protected abstract void ExpireEffect(Enemy enemy);

  public bool IsExpired => duration <= 0;
}
