using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Strongly-typed VFX Manager that provides type-safe spawning with compile-time validation
/// Mid-sized studio approach: Generic constraints + Parameter structs for scalability
/// </summary>
public class VFXManager : MonoBehaviour
{
  [Header("VFX Prefabs")]
  [SerializeField] private List<GameObject> vfxPrefabs;

  [Header("Pool Settings")]
  [SerializeField] private int initialPoolSize = 10;

  private int activeEffects = 0;
  private Transform poolParent;

  private Dictionary<Type, IVFXPool> vfxPools = new();

  public event Action OnAllEffectsFinished;

  private void Awake()
  {
    poolParent = new GameObject("VFX Pool").transform;
    poolParent.SetParent(transform);

    InitializePools();
  }


  private void InitializePools()
  {
    vfxPools.Clear();
    if (vfxPrefabs != null)
    {
      foreach (var prefab in vfxPrefabs)
      {
        if (prefab != null)
        {
          RegisterVFXPrefab(prefab);
        }
      }
    }
  }

  /// <summary>
  /// Register a VFX prefab - FULLY GENERIC using reflection
  /// Automatically detects any BaseVFX<TParams> component and creates the correct pool
  /// No manual registration needed - just add the prefab!
  /// </summary>
  private void RegisterVFXPrefab(GameObject prefab)
  {
    // Find any component that inherits from PooledVFX
    var pooledVFXComponents = prefab.GetComponents<PooledVFX>();

    foreach (var component in pooledVFXComponents)
    {
      var componentType = component.GetType();

      var baseType = componentType.BaseType;
      if (baseType != null && baseType.IsGenericType &&
          baseType.GetGenericTypeDefinition() == typeof(BaseVFX<>))
      {
        // Extract the TParams type from BaseVFX<TParams>
        var paramsType = baseType.GetGenericArguments()[0];

        // Create VFXPool<T, TParams> using reflection
        var poolType = typeof(VFXPool<,>).MakeGenericType(componentType, paramsType);
        var pool = Activator.CreateInstance(poolType, prefab, poolParent, initialPoolSize) as IVFXPool;

        vfxPools[componentType] = pool;
        Debug.Log($"✅ Registered VFX Pool for {componentType.Name}<{paramsType.Name}> with {initialPoolSize} instances");
        return;
      }
    }

    Debug.LogWarning($"Prefab {prefab.name} does not have a BaseVFX<TParams> component!");
  }

  #region Effect Registration

  /// <summary>
  /// Call when an effect (missile, particle, animation, etc.) starts
  /// </summary>
  public void RegisterEffect()
  {
    activeEffects++;
  }

  /// <summary>
  /// Call when an effect finishes
  /// </summary>
  public void UnregisterEffect()
  {
    activeEffects--;

    if (activeEffects <= 0)
    {
      activeEffects = 0;
      OnAllEffectsFinished?.Invoke();
    }
  }

  public bool AllEffectsFinished()
  {
    return activeEffects <= 0;
  }

  #endregion

  #region Type-Safe VFX Spawning

  /// <summary>
  /// Spawn a VFX with strongly-typed parameters
  /// </summary>
  public T SpawnVFX<T, TParams>(TParams spawnParams)
    where T : BaseVFX<TParams>
    where TParams : IVFXSpawnParams
  {
    if (vfxPools.TryGetValue(typeof(T), out var pool))
    {
      if (pool is VFXPool<T, TParams> typedPool)
      {
        var vfx = typedPool.GetTyped();
        if (vfx != null)
        {
          vfx.OnSpawn(spawnParams);
          return vfx;
        }
      }
    }

    Debug.LogError($"VFX Pool for {typeof(T).Name} not found! Did you add the prefab to VFXManager?");
    return null;
  }

  #endregion

  #region Cleanup

  private void OnDestroy()
  {
    foreach (var pool in vfxPools.Values)
    {
      pool.Clear();
    }
  }

  #endregion
}
