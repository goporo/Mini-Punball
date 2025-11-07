using UnityEngine;

/// <summary>
/// Base class for all pooled VFX effects
/// Handles lifecycle management with the VFX pool
/// </summary>
public abstract class PooledVFX : MonoBehaviour
{
  protected object pool;
  protected bool isActive = false;

  /// <summary>
  /// Set the pool that manages this VFX - supports strongly-typed pools
  /// </summary>
  public void SetPool(object vfxPool)
  {
    pool = vfxPool;
  }

  /// <summary>
  /// Called when the VFX is spawned from the pool
  /// Override this to initialize your VFX
  /// </summary>
  public virtual void OnSpawn()
  {
    isActive = true;
    GameContext.Instance.VFXManager.RegisterEffect();
  }

  /// <summary>
  /// Called when the VFX is returned to the pool
  /// Override this to clean up your VFX
  /// </summary>
  public virtual void OnDespawn()
  {
    isActive = false;
    GameContext.Instance.VFXManager.UnregisterEffect();
  }

  /// <summary>
  /// Return this VFX to the pool
  /// </summary>
  protected void ReturnToPool()
  {
    if (pool == null)
    {
      Debug.LogWarning($"VFX {gameObject.name} has no pool, destroying instead");
      Destroy(gameObject);
      return;
    }

    OnDespawn();

    if (pool is IVFXPool vfxPool)
    {
      vfxPool.Return(this);
    }
    else
    {
      Debug.LogWarning($"Pool for {gameObject.name} does not implement IVFXPool, cannot return to pool.");
      Destroy(gameObject);
    }
  }


  protected virtual void OnDisable()
  {
    isActive = false;
  }
}
