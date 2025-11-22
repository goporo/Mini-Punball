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
  /// Automatically detects any VFXBase<TParams> component and creates the correct pool
  /// No manual registration needed - just add the prefab!
  /// </summary>
  private void RegisterVFXPrefab(GameObject prefab)
  {
    // Find any component that inherits from PooledVFX
    var pooledVFXComponents = prefab.GetComponents<PooledVFX>();

    foreach (var component in pooledVFXComponents)
    {
      var componentType = component.GetType();

      // Walk up the inheritance chain to find VFXBase<TParams>
      var currentType = componentType;
      while (currentType != null && currentType != typeof(object))
      {
        if (currentType.IsGenericType && currentType.GetGenericTypeDefinition() == typeof(VFXBase<>))
        {
          // Extract the TParams type from VFXBase<TParams>
          var paramsType = currentType.GetGenericArguments()[0];

          // Create VFXPool<T, TParams> using reflection
          var poolType = typeof(VFXPool<,>).MakeGenericType(componentType, paramsType);
          var pool = Activator.CreateInstance(poolType, prefab, poolParent, initialPoolSize) as IVFXPool;

          vfxPools[componentType] = pool;
          Debug.Log($"✅ Registered VFX Pool for {componentType.Name}<{paramsType.Name}> with {initialPoolSize} instances");
          return;
        }
        currentType = currentType.BaseType;
      }
    }

    Debug.LogWarning($"Prefab {prefab.name} does not have a VFXBase<TParams> component!");
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
    where T : VFXBase<TParams>
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

  /// <summary>
  /// Spawn a VFX by class type (non-generic, uses reflection)
  /// </summary>
  public void SpawnVFXByClassType(System.Type vfxType, IVFXSpawnParams spawnParams)
  {
    if (vfxPools.TryGetValue(vfxType, out var pool))
    {
      // Find the GetTyped method via reflection
      var poolType = pool.GetType();
      var getTypedMethod = poolType.GetMethod("GetTyped");
      if (getTypedMethod != null)
      {
        var vfxInstance = getTypedMethod.Invoke(pool, null);
        if (vfxInstance != null)
        {
          // Find OnSpawn method
          var onSpawnMethod = vfxInstance.GetType().GetMethod("OnSpawn");
          if (onSpawnMethod != null)
          {
            onSpawnMethod.Invoke(vfxInstance, new object[] { spawnParams });
            return;
          }
        }
      }
    }
    Debug.LogError($"VFX Pool for {vfxType.Name} not found or could not spawn! Did you add the prefab to VFXManager?");
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
