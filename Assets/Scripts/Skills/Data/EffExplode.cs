using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "MiniPunBall/Skill/EffExplode")]
public class EffExplode : EffectSO<EffectContext>
{
  [SerializeField] private int multiplier = 6;
  private int radius = 1;

  public override void Execute(EffectContext ctx)
  {
    var targets = LevelContext.Instance.BoardState.GetSurroundingObjects(ctx.Enemy.CurrentCell, radius);
    var enemies = targets.OfType<Enemy>().ToList();
    EventBus.Publish(new OnBombExplodeEvent(new AOEContext(enemies)));

    LevelContext.Instance.VFXManager.SpawnVFX<VFXExplode, BasicVFXParams>(
      new BasicVFXParams
      {
        Position = ctx.Enemy.Position,
      }
    );

    foreach (var obj in targets)
    {
      if (obj == null) continue;
      if (obj.TryGetComponent<Enemy>(out var target))
      {
        var dmgCtx = DamageContext.CreateEffectDamage(
            target,
            multiplier,
            DamageType.Explosion
        );
        CombatResolver.Instance.ResolveHit(dmgCtx);
      }
      else if (obj.TryGetComponent<IDamageable>(out var damageable))
      {
        var dmgCtx = DamageContext.CreateObstacleDamage();
        damageable.TakeDamage(dmgCtx);
      }
    }
  }

}


