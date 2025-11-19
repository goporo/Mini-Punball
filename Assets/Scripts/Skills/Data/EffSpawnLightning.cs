using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MiniPunBall/Skill/EffSpawnLightning", order = 0)]
public class EffSpawnLightning : EffectSO<EffectContext>
{
  [SerializeField] private int multiplier = 2;
  [SerializeField] private int maxTargets = 2;

  public override void Execute(EffectContext ctx)
  {
    var targets = LevelContext.Instance.BoardState.GetRandomEnemies(maxTargets, ctx.Enemy);

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
            StartPoint = ctx.Enemy.Position,
            EndPoint = target.Position,
          }
        );
        CombatResolver.Instance.ResolveHit(dmgCtx);
      }
    }
  }
}


