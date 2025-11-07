using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Generic object pool for VFX effects with strongly-typed parameters
/// </summary>
/// <typeparam name="T">Type of pooled VFX component</typeparam>
/// <typeparam name="TParams">Type of spawn parameters</typeparam>
public class VFXPool<T, TParams> : IVFXPool
  where T : BaseVFX<TParams>
  where TParams : IVFXSpawnParams
{
  public System.Type VFXType => typeof(T);

  private readonly Queue<T> pool = new();
  private readonly GameObject prefab;
  private readonly Transform parent;

  /// <summary>
  /// Public constructor for reflection-based instantiation
  /// </summary>
  public VFXPool(GameObject prefab, Transform parent, int initialSize = 5)
  {
    this.prefab = prefab;
    this.parent = parent;

    for (int i = 0; i < initialSize; i++)
    {
      var instance = CreateNewInstance();
      if (instance != null)
        pool.Enqueue(instance);
    }
  }


  /// <summary>
  /// Get an instance from the pool (IVFXPool interface)
  /// </summary>
  public object Get()
  {
    return GetTyped();
  }

  /// <summary>
  /// Get a typed instance from the pool
  /// </summary>
  public T GetTyped()
  {
    T instance;
    if (pool.Count > 0)
    {
      instance = pool.Dequeue();
    }
    else
    {
      instance = CreateNewInstance();
    }

    instance.gameObject.SetActive(true);
    return instance;
  }


  public void Return(object instance)
  {
    if (instance is T typedInstance)
    {
      typedInstance.gameObject.SetActive(false);
      pool.Enqueue(typedInstance);
    }
    else
    {
      Debug.LogWarning($"Returned instance is not of type {typeof(T).Name}");
    }
  }

  /// <summary>
  /// Create a new instance for the pool
  /// </summary>
  private T CreateNewInstance()
  {
    var obj = Object.Instantiate(prefab, parent);

    if (!obj.TryGetComponent<T>(out var component))
    {
      Debug.LogError($"Prefab {prefab.name} does not have component {typeof(T).Name}");
      Object.Destroy(obj);
      return null;
    }

    component.SetPool(this);
    obj.SetActive(false);
    return component;
  }

  /// <summary>
  /// Clear all pooled instances
  /// </summary>
  public void Clear()
  {
    while (pool.Count > 0)
    {
      var instance = pool.Dequeue();
      if (instance != null)
      {
        Object.Destroy(instance.gameObject);
      }
    }
  }


}
