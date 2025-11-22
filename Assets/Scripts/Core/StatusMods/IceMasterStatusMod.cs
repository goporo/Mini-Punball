public class IceMasterStatusMod : IStatusModifier
{
  public void Process(StatusExecutionContext ctx)
  {
    if (ctx.StatusEffect is not StatusFrozen status)
      return;
    status.IncreaseChance(1.5f);
    status.IncreaseDuration(1);
  }
}
