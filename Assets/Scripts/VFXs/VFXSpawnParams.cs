using UnityEngine;

/// <summary>
/// Base interface for all VFX spawn parameters
/// This ensures type safety and makes parameters discoverable
/// </summary>
public interface IVFXSpawnParams { }

public struct BasicVFXParams : IVFXSpawnParams
{
  public Vector3 Position { get; set; }

}

public struct LaserVFXParams : IVFXSpawnParams
{
  public Vector3 StartPoint { get; set; }
  public Vector3 EndPoint { get; set; }

}

public struct LightningVFXParams : IVFXSpawnParams
{
  public Vector3 StartPoint { get; set; }
  public Vector3 EndPoint { get; set; }

}

public struct TargetVFXParams : IVFXSpawnParams
{
  public Vector3 Position { get; set; }
  public Enemy Target { get; set; }
  public System.Action Callback { get; set; }

}
