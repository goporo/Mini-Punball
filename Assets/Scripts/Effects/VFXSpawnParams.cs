using UnityEngine;

/// <summary>
/// Base interface for all VFX spawn parameters
/// This ensures type safety and makes parameters discoverable
/// </summary>
public interface IVFXSpawnParams
{
}


public struct BasicVFXParams : IVFXSpawnParams
{
  public Vector3 Position { get; set; }

  public BasicVFXParams(Vector3 position)
  {
    Position = position;
  }
}

public struct LaserVFXParams : IVFXSpawnParams
{
  public Vector3 StartPoint { get; set; }
  public Vector3 EndPoint { get; set; }

  public LaserVFXParams(Vector3 startPoint, Vector3 endPoint)
  {
    StartPoint = startPoint;
    EndPoint = endPoint;
  }
}

public struct MissileVFXParams : IVFXSpawnParams
{
  public Vector3 Position { get; set; }
  public Enemy Target { get; set; }
  public System.Action Callback { get; set; }

  public MissileVFXParams(Vector3 position, Enemy target, System.Action callback)
  {
    Position = position;
    Target = target;
    Callback = callback;
  }
}
