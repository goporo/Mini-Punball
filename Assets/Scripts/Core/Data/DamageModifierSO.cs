using UnityEngine;

public abstract class DamageModifierSO : ScriptableObject, IDamageModifier
{
  public abstract void Apply(DamageContext ctx);
}
