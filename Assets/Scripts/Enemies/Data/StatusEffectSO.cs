using UnityEngine;

public abstract class StatusEffectSO : ScriptableObject
{
  [Tooltip("Duration in rounds")]
  [SerializeField] protected int duration;

  [Tooltip("Chance to trigger (0-1)")]
  [SerializeField] protected float triggerChance = 1f;
  protected abstract IStatusEffect CreateRuntimeInstance();

  public void ApplyTo(Enemy enemy)
  {
    var runtime = CreateRuntimeInstance();
    enemy.StatusController.AddEffect(runtime);  // this handles Apply()
  }
}
