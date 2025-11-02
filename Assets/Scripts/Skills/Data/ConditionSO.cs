using UnityEngine;

public abstract class ConditionSO : ScriptableObject
{
  public abstract bool Evaluate(IEffectContext ctx);
}

public abstract class ConditionSO<TContext> : ConditionSO where TContext : IEffectContext
{
  public abstract bool Evaluate(TContext ctx);

  public sealed override bool Evaluate(IEffectContext ctx)
  {
    if (ctx is TContext typedCtx)
      return Evaluate(typedCtx);

    return false; // Condition fails if context type doesn't match
  }
}

