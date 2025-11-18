using UnityEngine;


/// <summary>
/// Generic interface for strongly-typed VFX
/// </summary>
public interface IVFXBase<TParams> where TParams : IVFXSpawnParams
{
  void OnSpawn(TParams spawnParams);
}

/// <summary>
/// Interface for VFX pool abstraction
/// </summary>
public interface IVFXPool
{
  System.Type VFXType { get; }
  object Get();
  void Clear();
  void Return(object instance);
}


