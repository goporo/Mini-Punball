using UnityEngine;

public class VFXBurn : VFXBase<BasicVFXParams>
{
  private bool isPersistent = true;   // fire stays active until StopVFX() is called


  void Start()
  {
    PlayParticleSystems();
  }
  public override void OnSpawn(BasicVFXParams spawnParams)
  {
    base.OnSpawn();
    transform.position = spawnParams.Position;
    PlayParticleSystems();
  }

  /// <summary>
  /// Call this from your BurnEffect system when burn expires.
  /// </summary>
  public void StopVFX()
  {
    isPersistent = false;
    StopParticleSystems();
  }

  private void Update()
  {
    if (!isActive) return;

    // Only check for completion AFTER StopVFX() was called
    if (!isPersistent && AreAllParticlesStopped())
    {
      RequestDone();
    }
  }

  protected void PlayParticleSystems()
  {
    if (particleSystems == null) return;

    foreach (var ps in particleSystems)
      ps?.Play();
  }

  protected void StopParticleSystems()
  {
    if (particleSystems == null) return;

    foreach (var ps in particleSystems)
      ps?.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
  }

  protected bool AreAllParticlesStopped()
  {
    if (particleSystems == null || particleSystems.Length == 0)
      return true;

    foreach (var ps in particleSystems)
      if (ps != null && ps.IsAlive(true))
        return false;

    return true;
  }
}
