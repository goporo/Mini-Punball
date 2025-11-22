using System;
using System.Collections.Generic;
using UnityEngine;

public class StatusExecutor : Singleton<StatusExecutor>
{
  private readonly List<IStatusModifier> modifiers = new();

  public void AddModifier(IStatusModifier modifier)
  {
    modifiers.Add(modifier);
  }

  public void Execute(StatusEffectSO effect, Enemy enemy)
  {
    // create runtime copy so SO asset is never mutated
    var runtimeStatus = ScriptableObject.Instantiate(effect);

    var execCtx = new StatusExecutionContext
    {
      StatusEffect = runtimeStatus,
      Target = enemy
    };

    foreach (var mod in modifiers)
      mod.Process(execCtx);

    execCtx.StatusEffect.ApplyEffect(enemy);
  }
}
