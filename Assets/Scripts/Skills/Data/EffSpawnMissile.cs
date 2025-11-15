using UnityEngine;

[CreateAssetMenu(menuName = "MiniPunBall/Skill/EffSpawnMissile", order = 0)]
public class EffSpawnMissile : EffectSO<EffectContext>
{
  public int count = 1;
  public int multiplier = 1;
  public GameObject missilePrefab;
  public override void Execute(EffectContext ctx)
  {
    var targets = LevelContext.Instance.BoardState.GetLowestHealthEnemies(count, ctx.Enemy);

    for (int i = 0; i < targets.Count; i++)
    {

      var target = targets[i];
      if (target != null)
      {
        var damage = ctx.Player.Stats.Attack * multiplier;
        LevelContext.Instance.VFXManager.SpawnVFX<MissileVFX, MissileVFXParams>(
          new MissileVFXParams
          {
            Position = ctx.Enemy.Position,
            Target = target,
            Callback = () => CombatResolver.Instance.ResolveEffectHit(
              new ResolveEffectHitContext(target, damage, null, DamageType.Missile)
            )
          }
    );
      }
    }
  }
}


