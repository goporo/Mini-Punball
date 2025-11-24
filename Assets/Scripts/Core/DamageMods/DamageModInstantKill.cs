using UnityEngine;

[CreateAssetMenu(menuName = "MiniPunBall/DamageModifier/InstantKill")]
public class DamageModInstantKill : DamageModifierSO
{
  [SerializeField] private float chance = 0.15f;

  public override void Apply(DamageContext ctx)
  {
    if (Random.value < chance)
    {
      ctx.FinalDamage = 999999; // Instant kill
    }

  }
}
