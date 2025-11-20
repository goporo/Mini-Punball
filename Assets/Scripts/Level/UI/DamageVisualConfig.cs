using UnityEngine;

public class DamageVisualConfig
{
  public static Color GetColor(DamageType damageType)
  {
    switch (damageType)
    {
      case DamageType.Normal:
        return Color.white;
      case DamageType.Fire:
        return Color.red;
      case DamageType.Ice:
        return Color.cyan;
      case DamageType.Lightning:
        return Color.magenta;
      case DamageType.Laser:
        return Color.yellow;
      case DamageType.Missile:
        return Color.black;
      case DamageType.Explosion:
        return Color.black;
      case DamageType.Heal:
        return Color.green;
      case DamageType.Void:
        return Color.black;
      case DamageType.Split:
        return Color.cyan;
      case DamageType.Drill:
        return Color.magenta;
      default:
        return Color.white;
    }
  }
}