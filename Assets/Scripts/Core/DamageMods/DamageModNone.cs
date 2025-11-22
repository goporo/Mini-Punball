using UnityEngine;

[CreateAssetMenu(menuName = "MiniPunBall/DamageModifier/None")]
public class DamageModifierNone : DamageModifierSO
{
  public override void Apply(DamageContext ctx)
  {
    // No modification
  }
}