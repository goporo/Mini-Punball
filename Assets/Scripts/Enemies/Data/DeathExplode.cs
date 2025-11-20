using UnityEngine;

[CreateAssetMenu(menuName = "MiniPunBall/Enemy/DeathEffect/Explode")]
public class ExplodeDeath : DeathEffect
{
  [SerializeField] private int radius = 1;
  [SerializeField] private float damageByHealthRatio = 0.5f;

  public override void OnDeath(Enemy enemy, BoardState board)
  {
    var targets = board.GetSurroundingObjects(enemy.CurrentCell, radius);

    LevelContext.Instance.VFXManager.SpawnVFX<VFXExplode, BasicVFXParams>(
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
        var ctx = DamageContext.CreateFixedDamage(
            target,
            amount,
            DamageType.Explosion
        );
        CombatResolver.Instance.ResolveHit(ctx);
      }
      else if (obj.TryGetComponent<IDamageable>(out var damageable))
      {
        var dmgCtx = DamageContext.CreateObstacleDamage();
        damageable.TakeDamage(dmgCtx);
      }
    }
  }
}