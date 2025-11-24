using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MiniPunBall/Skill/EffSpawnLightning")]
public class EffSpawnLightning : EffectSO<EffectCastContext>
{
  [SerializeField] private int multiplier = 2;
  [SerializeField] private int maxTargets = 2;

  public override void Execute(EffectCastContext ctx)
  {
    var originInstance = CastOriginFactory.GetCastInstance(ctx.CastSource);
    var targets = LevelContext.Instance.BoardState.GetRandomEnemies(maxTargets, ctx.CastEnemy);
    if (targets == null || targets.Count == 0) return;
    var spawnPos = CastOriginFactory.GetGroundOrigin(ctx.CastEnemy);

    for (int i = 0; i < targets.Count; i++)
    {
      var target = targets[i];
      if (target != null)
      {
        var damage = multiplier;
        var dmgCtx = DamageContext.CreateEffectDamage(
          target,
          damage,
          DamageType.Lightning
        );
        LevelContext.Instance.VFXManager.SpawnVFX<VFXLightning, LightningVFXParams>(
          new LightningVFXParams
          {
            StartPoint = spawnPos,
            EndPoint = target.Position,
          }
        );
        CombatResolver.Instance.ResolveHit(dmgCtx);
      }
    }
    originInstance.OnSpawn(this);
  }

  public void IncreaseCount(int amount)
  {
    maxTargets += amount;
  }

}


