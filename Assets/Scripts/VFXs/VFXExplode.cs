using UnityEngine;

public class VFXExplode : VFXBase<BasicVFXParams>
{
  public override void OnSpawn(BasicVFXParams spawnParams)
  {
    base.OnSpawn();
    transform.position = spawnParams.Position;
    PlayParticleSystems();
  }

  private void Update()
  {
    if (!isActive) return;
    if (AreAllParticlesStopped())
    {
      RequestDone();
    }
  }

  protected void PlayParticleSystems()
  {
    if (particleSystems == null) return;
    foreach (var ps in particleSystems)
    {
      ps?.Play();
    }
  }

  protected bool AreAllParticlesStopped()
  {
    if (particleSystems == null || particleSystems.Length == 0) return true;
    for (int i = 0, len = particleSystems.Length; i < len; i++)
    {
      var ps = particleSystems[i];
      if (ps != null && ps.IsAlive(true))
        return false;
    }
    return true;
  }
}