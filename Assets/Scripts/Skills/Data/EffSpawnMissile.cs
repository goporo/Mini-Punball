using UnityEngine;

[CreateAssetMenu(menuName = "MiniPunBall/Skill/EffSpawnMissile", order = 0)]
public class EffSpawnMissile : EffectSO<BallHitContext>
{
  public GameObject missilePrefab;
  public override void Execute(BallHitContext ctx)
  {
    // loi tween destroy
    var missile = Instantiate(missilePrefab, ctx.Enemy.Position, Quaternion.identity).GetComponent<Missile>();
    var target = GameContext.Instance.BoardState.GetLowestHealthEnemy(ctx.Enemy);
    if (target != null)
    {
      var damageCtx = new DamageContext(ctx.Damage, null);
      missile.SetTarget(target, () => target.HealthComponent.TakeDamage(damageCtx));
    }


  }
}


