using System;
using UnityEngine;

public abstract class TriggerSO : ScriptableObject
{
  public abstract IDisposable Subscribe(SkillRuntime runtime, Action<IEffectContext> fire);
}

public abstract class TriggerSO<TContext> : TriggerSO where TContext : IEffectContext
{
  public abstract IDisposable Subscribe(SkillRuntime skill, Action<TContext> fire);

  public sealed override IDisposable Subscribe(SkillRuntime runtime, Action<IEffectContext> fire)
  {
    return Subscribe(runtime, ctx => fire(ctx));
  }
}

public enum TriggerType
{
  OnHit,
  OnEnemyDeath,
  OnSkillGain
}

