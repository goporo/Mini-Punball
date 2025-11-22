using UnityEngine;

public class StatusEffectSO : ScriptableObject
{
  [Tooltip("Type of status effect")]
  [SerializeField] private StatusEffectType type;
  public StatusEffectType Type => type;

  [Tooltip("Duration in rounds")]
  [SerializeField] protected int duration;

  [Tooltip("Chance to trigger (0-1)")]
  [SerializeField] protected float triggerChance = 1f;

  protected virtual StatusEffectBase CreateRuntimeInstance()
  {
    // Default implementation returns null. Override in derived classes for custom behavior.
    return null;
  }

  public void ApplyEffect(Enemy enemy)
  {
    var runtime = CreateRuntimeInstance();
    if (runtime != null)
      enemy.StatusController.AddEffect(runtime);
  }
}
