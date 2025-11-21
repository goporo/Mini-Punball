using UnityEngine;

[CreateAssetMenu(menuName = "MiniPunBall/DamageModifier/Backstab")]
public class BackstabModifierSO : DamageModifierSO
{
  [SerializeField] private int multiplier = 15;

  public override void Apply(DamageContext ctx)
  {
    if (ctx.Hitbox != HitboxType.Back) return;
    ctx.FinalDamage *= multiplier;
  }
}
