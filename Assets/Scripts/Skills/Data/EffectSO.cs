using UnityEngine;

public abstract class EffectSO : ScriptableObject
{
  public abstract void Execute(IEffectContext ctx);
}

public abstract class EffectSO<TContext> : EffectSO where TContext : IEffectContext
{
  public abstract void Execute(TContext ctx);

  public sealed override void Execute(IEffectContext ctx)
  {
    if (ctx is TContext typedCtx)
      Execute(typedCtx);
  }
}

