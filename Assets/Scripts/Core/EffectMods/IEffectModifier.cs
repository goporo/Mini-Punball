public interface IEffectModifier
{
  void Process(EffectExecutionContext ctx);
}

public class EffectExecutionContext
{
  public EffectSO<EffectContext> Effect;
  public EffectContext Context;
}