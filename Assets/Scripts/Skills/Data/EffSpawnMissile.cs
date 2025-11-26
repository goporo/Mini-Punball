using UnityEngine;

[CreateAssetMenu(menuName = "MiniPunBall/Skill/EffSpawnMissile")]
public class EffSpawnMissile : EffectSO<EffectCastContext>
{
  [SerializeField] private int multiplier = 1;
  [SerializeField] private int maxTargets = 1;
  public override void Execute(EffectCastContext ctx)
  {
    var targets = LevelContext.Instance.BoardState.GetLowestHealthEnemies(maxTargets, ctx.CastEnemy);
    var spawnPos = ctx.CastEnemy != null ? ctx.CastEnemy.Position : CastOriginFactory.GetDefaultEnemySpawnOrigin();

    for (int i = 0; i < targets.Count; i++)
    {
      var target = targets[i];
      if (target != null)
      {
        var damage = multiplier;
        var dmgCtx = DamageContext.CreateEffectDamage(
          target,
          damage,
          DamageType.Missile
        );
        LevelContext.Instance.VFXManager.SpawnVFX<VFXMissile, TargetVFXParams>(
          new TargetVFXParams
          {
            Position = spawnPos,
            Target = target,
            Callback = () => CombatResolver.Instance.ResolveHit(dmgCtx)
          }
        );
      }
    }
  }

  public void IncreaseCount(int amount)
  {
    maxTargets += amount;
  }
}


