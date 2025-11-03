using UnityEngine;

[CreateAssetMenu(menuName = "MiniPunBall/Skill/EffSpawnMissile", order = 0)]
public class EffSpawnMissile : EffectSO<BallHitContext>
{
  public int count = 1;
  public int multiplier = 1;
  public GameObject missilePrefab;
  public override void Execute(BallHitContext ctx)
  {
    var targets = GameContext.Instance.BoardState.GetLowestHealthEnemies(count, ctx.Enemy);

    for (int i = 0; i < targets.Count; i++)
    {
      var missile = Instantiate(missilePrefab, ctx.Enemy.Position, Quaternion.identity).GetComponent<Missile>();
      var target = targets[i];
      if (target != null)
      {
        var damageCtx = new DamageContext(ctx.Player.Stats.Attack * multiplier, null);
        missile.SetTarget(target, () => target.HealthComponent.TakeDamage(damageCtx));
      }
    }
  }
}


