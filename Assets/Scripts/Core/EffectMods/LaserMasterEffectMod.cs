public class LaserMasterEffectMod : IEffectModifier
{
  public void Process(EffectExecutionContext ctx)
  {
    if (ctx.Effect is not EffSpawnLaser laser)
      return;

    laser.SetDirection(EffSpawnLaser.Direction.Both);
  }
}
