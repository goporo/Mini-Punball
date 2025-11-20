using UnityEngine;

[CreateAssetMenu(menuName = "MiniPunBall/Skill/EffDropFireball")]
public class EffDropFireball : EffectSO<EffectContext>
{
  [SerializeField] private int count = 1;
  [SerializeField] private int multiplier = 1;
  [SerializeField] private GameObject missilePrefab;
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
        LevelContext.Instance.VFXManager.SpawnVFX<VFXMissile, MissileVFXParams>(
          new MissileVFXParams
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


