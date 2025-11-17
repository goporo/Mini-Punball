using UnityEngine;

public class Hitbox : MonoBehaviour
{
  public Enemy Enemy;
  [SerializeField] private HitboxType hitBoxType;
  public HitboxType Type => hitBoxType;

  public void OnHit(DamageContext ctx)
  {
    CombatResolver.Instance.ResolveHit(ctx);
  }
}

public enum HitboxType
{
  Front,
  Back,
  Side,
  Center
}
