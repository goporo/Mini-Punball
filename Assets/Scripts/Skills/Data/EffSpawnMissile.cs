using UnityEngine;

[CreateAssetMenu(menuName = "MiniPunBall/Skill/EffSpawnMissile")]
public class EffSpawnMissile : EffectSO<EffectContext>
{
  [SerializeField] private int count = 1;
  [SerializeField] private int multiplier = 1;
  public override void Execute(EffectContext ctx)
  {
    var targets = LevelContext.Instance.BoardState.GetLowestHealthEnemies(count, ctx.Enemy);

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
            Position = ctx.Enemy.Position,
            Target = target,
            Callback = () => CombatResolver.Instance.ResolveHit(dmgCtx)
          }
        );
      }
    }
  }
}


