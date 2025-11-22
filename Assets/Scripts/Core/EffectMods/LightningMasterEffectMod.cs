public class LightningMasterEffectMod : IEffectModifier
{
  public void Process(EffectExecutionContext ctx)
  {
    if (ctx.Effect is not EffSpawnLightning lightning)
      return;

    lightning.IncreaseCount(2);
  }
}
