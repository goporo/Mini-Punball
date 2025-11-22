public interface IStatusModifier
{
  void Process(StatusExecutionContext ctx);
}

public class StatusExecutionContext
{
  public StatusEffectSO StatusEffect;
  public Enemy Target;
}