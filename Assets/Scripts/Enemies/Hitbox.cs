using UnityEngine;

public class Hitbox : MonoBehaviour
{
  public Enemy Enemy;
  public float damageMultiplier = 1f;
  [SerializeField] private HitboxType hitBoxType;

  public void OnHit(ResolveBallHitContext ctx)
  {
    ctx.HitboxType = hitBoxType;
    CombatResolver.Instance.ResolveBallHit(ctx);
  }
}

public enum HitboxType
{
  Front,
  Back,
  Side,
  Center
}
