using UnityEngine;

public struct HitResult
{
  public int finalDamage;
  public bool killed;
}

public struct ResolveHitContext
{
  public PlayerRunStats Player => GameContext.Instance.Player;
  public Enemy Enemy;
  public BallBase Ball;
}

public class CombatResolver : Singleton<CombatResolver>
{
  public HitResult ResolveHit(ResolveHitContext ctx)
  {
    // 1) compute damage (from player stats, ball stats, skills, etc.)
    int damage = ComputeDamage(ctx);
    var dmgCtx = new DamageContext
    {
      amount = damage,
      statusEffect = ctx.Ball.Stats.statusEffect
    };

    // 2) apply to health (keep health logic minimal & local)
    bool killed = ctx.Enemy.HealthComponent.TakeDamage(dmgCtx);

    // 3) apply statuses from ball (burn/slow/etc.)
    var effectCtx = new BallHitContext(ctx.Enemy, damage, ctx.Ball);
    ctx.Ball.Stats.onHitEffect?.Execute(effectCtx);
    // ctx.Ball.StatusApplier?.ApplyOnHit(ctx.Enemy);

    // 4) update global combo (if you count each contact)
    GameContext.Instance.ComboManager.Increment(1);

    // 5) publish ONE rich event for reactions (skills, UI, sounds)
    // var evt = new OnHitEvent(
    //     ctx.player, ctx.ball, ctx.enemy, ctx.hitPoint, damage, killed);
    // EventBus.Publish(evt);

    // if (killed)
    //   EventBus.Publish(new EnemyDeathEvent(ctx.enemy, ctx.hitPoint));

    return new HitResult { finalDamage = damage, killed = killed };
  }

  private int ComputeDamage(ResolveHitContext ctx)
  {
    var baseDmg = Mathf.RoundToInt(ctx.Player.Stats.Attack * ctx.Ball.Stats.BaseDamage);
    // apply first/last-ball, elemental, etc. (read-only)
    return baseDmg;
  }
}
