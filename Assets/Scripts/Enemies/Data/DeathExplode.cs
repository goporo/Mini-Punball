using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MiniPunBall/Enemy/DeathEffect/Explode")]
public class ExplodeDeath : DeathEffect
{
  [SerializeField] private int radius = 1;
  [SerializeField] private float damageByHealthRatio = 0.5f;

  public override void OnDeath(Enemy enemy, BoardState board)
  {
    Debug.Log($"{enemy.name} exploded!");
    var targets = board.GetSurroundingObjects(enemy.CurrentCell, radius);

    // Spawn explosion VFX from centralized VFXManager pool (generic)
    GameContext.Instance.VFXManager.SpawnVFX<ExplodeVFX, BasicVFXParams>(
      new BasicVFXParams
      {
        Position = enemy.Position,
      }
    );

    foreach (var obj in targets)
    {
      if (obj == null) continue;
      if (obj.TryGetComponent<Enemy>(out var target))
      {
        var amount = Mathf.RoundToInt(enemy.Stats.Health * damageByHealthRatio);
        CombatResolver.Instance.ResolveEffectHit(
          new ResolveEffectHitContext(target, amount, null, DamageType.Explosion)
        );
      }
      else if (obj.TryGetComponent<IDamageable>(out var damageable))
      {
        damageable.TakeDamage(new DamageContext { });
      }
    }
  }
}