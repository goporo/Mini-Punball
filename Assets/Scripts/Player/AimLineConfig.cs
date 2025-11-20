using UnityEngine;

[System.Serializable]
public class AimLineConfig
{
  [Tooltip("Minimum Z position for aiming (line is clamped to this)")]
  public float baseLineZ = -3.3f;
  public float CancelLineZ = -5.0f;
  public Vector3 LeftMostPoint => new(-0.5f, 0f, baseLineZ);
  public Vector3 RightMostPoint => new(5.5f, 0f, baseLineZ);

}
