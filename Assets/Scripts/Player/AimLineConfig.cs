using UnityEngine;

[System.Serializable]
public class AimLineConfig
{
  [Tooltip("Minimum Z position for aiming (line is clamped to this)")]
  public float baseLineZ = -3.3f;

}
