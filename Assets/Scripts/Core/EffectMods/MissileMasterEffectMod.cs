public class MissileMasterEffectMod : IEffectModifier
{
  public void Process(EffectExecutionContext ctx)
  {
    if (ctx.Effect is not EffSpawnMissile missile)
      return;

    missile.IncreaseCount(1); // (+1 missile globally)
  }
}
