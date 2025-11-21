using UnityEngine;

[CreateAssetMenu(menuName = "MiniPunBall/DamageModifier/Distance")]
public class DamageModDistance : DamageModifierSO
{
  [SerializeField]
  private DistanceType distanceType;
  private enum DistanceType
  {
    Near,
    Far
  }
  [SerializeField] private int multiplier = 2;

  public override void Apply(DamageContext ctx)
  {
    int distance = ctx.Enemy.CurrentCell.y;

    switch (distanceType)
    {
      case DistanceType.Far:
        // Increase damage if hit far from player
        int newDamage = ctx.FinalDamage + multiplier * ctx.FinalDamage * distance;
        ctx.FinalDamage = newDamage;
        break;
      case DistanceType.Near:
        // Increase damage if hit near player
        newDamage = ctx.FinalDamage + multiplier * ctx.FinalDamage * (6 - distance);
        ctx.FinalDamage = newDamage;
        break;
    }
  }
}
