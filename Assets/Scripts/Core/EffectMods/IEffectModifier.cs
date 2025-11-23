public interface IEffectModifier
{
  void Process(EffectExecutionContext ctx);
}

public class EffectExecutionContext
{
  public EffectSO Effect;
  public IEffectContext Context;
}