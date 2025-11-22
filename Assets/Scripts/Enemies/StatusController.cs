using System.Collections.Generic;

public class StatusController
{
  private readonly Enemy enemy;
  private readonly List<StatusEffectBase> effects = new();

  // Priority dictionary: higher value = higher priority
  private static readonly Dictionary<StatusEffectType, int> effectPriorities = new()
  {
    { StatusEffectType.Burn, 1 },
    { StatusEffectType.Frozen, 2 }
    // Add more effect priorities here as needed
  };

  public StatusController(Enemy enemy)
  {
    this.enemy = enemy;
  }

  public List<StatusEffectBase> GetActiveEffects()
  {
    return effects;
  }

  public void AddEffect(StatusEffectBase effect)
  {
    bool isActive = effect.TryTrigger();
    if (!isActive) return;

    int newPriority = GetPriority(effect.EffectType);
    if (effects.Count > 0)
    {
      int currentPriority = GetPriority(effects[0].EffectType);
      if (newPriority >= currentPriority)
      {
        effects[0].OnExpire(enemy);
        effects.RemoveAt(0);
      }
      else
      {
        return; // Do not replace if new effect is lower priority
      }
    }

    effects.Add(effect);
    effect.OnApply(enemy);
  }

  private static int GetPriority(StatusEffectType type)
  {
    return effectPriorities.TryGetValue(type, out int priority) ? priority : 0;
  }

  public void OnRoundStart()
  {
    for (int i = effects.Count - 1; i >= 0; i--)
    {
      var e = effects[i];
      e.OnRound(enemy);
      if (e.IsExpired)
      {
        e.OnExpire(enemy);
        effects.RemoveAt(i);
      }
    }
  }
}
