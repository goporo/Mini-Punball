using System;
using UnityEngine;

/// <summary>
/// Base class for all VFX types, implements IBaseVFX for pooling and spawn logic.
/// </summary>
/// <typeparam name="TParams">The spawn parameter type for this VFX</typeparam>
public abstract class BaseVFX<TParams> : PooledVFX, IBaseVFX<TParams> where TParams : IVFXSpawnParams
{
  [Header("VFX Particle Systems")]
  [SerializeField] protected ParticleSystem[] particleSystems;
  private bool done = false;
  public bool IsDone => done;

  protected virtual void Awake()
  {
    if (particleSystems == null || particleSystems.Length == 0)
    {
      particleSystems = GetComponentsInChildren<ParticleSystem>();
    }
  }

  public override void OnDespawn()
  {
    base.OnDespawn();

    transform.localScale = Vector3.one;

    if (particleSystems != null)
    {
      foreach (var ps in particleSystems)
      {
        if (ps != null)
        {
          ps.Stop();
          ps.Clear();
        }
      }
    }
  }

  protected void RequestDone()
  {
    done = true;
    ReturnToPool();
  }



  public abstract void OnSpawn(TParams spawnParams);
}
