public class FireMasterStatusMod : IStatusModifier
{
  public void Process(StatusExecutionContext ctx)
  {
    if (ctx.StatusEffect is not StatusBurn status)
      return;

    status.SetSpreadable(true);
  }
}
