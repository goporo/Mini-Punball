using UnityEngine;

public class BaseDamageModifier : IDamageModifier
{
  public void Apply(DamageContext ctx)
  {
    // handle player attack not passed
    int playerAttack = Mathf.Max(ctx.PlayerAttack, LevelContext.Instance.Player.CurrentAttack);
    ctx.FinalDamage = (int)(playerAttack * ctx.BaseDamage);
  }
}
