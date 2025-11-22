using System;
using System.Collections.Generic;
using UnityEngine;

public class EffectExecutor : Singleton<EffectExecutor>
{
  private readonly List<IEffectModifier> modifiers = new();

  public void AddModifier(IEffectModifier modifier)
  {
    modifiers.Add(modifier);
  }

  public void Execute(EffectSO<EffectContext> effect, EffectContext ctx)
  {
    // create runtime copy so SO asset is never mutated
    var runtimeEffect = ScriptableObject.Instantiate(effect);

    var execCtx = new EffectExecutionContext
    {
      Effect = runtimeEffect,
      Context = ctx
    };

    foreach (var mod in modifiers)
      mod.Process(execCtx);

    execCtx.Effect.Execute(ctx);
  }
}
