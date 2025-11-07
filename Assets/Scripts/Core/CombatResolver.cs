using UnityEngine;

public struct HitResult
{
  public int finalDamage;
  public bool killed;
}

public class ResolveBallHitContext : IEffectContext
{
  public PlayerRunStats Player => GameContext.Instance.Player;
  public Enemy Enemy;
  public int BallDamage;
  public EffectSO<EffectContext> OnHitEffect;
  public IStatusEffect StatusEffect;
  public DamageType DamageType;
  public HitboxType HitboxType;

  public ResolveBallHitContext(Enemy enemy, int ballDamage, EffectSO<EffectContext> onHitEffect, DamageType damageType)
  {
    Enemy = enemy;
    BallDamage = ballDamage;
    OnHitEffect = onHitEffect;
    DamageType = damageType;
  }
  // Add more fields as needed
}


public class ResolveEffectHitContext : IEffectContext
{
  public PlayerRunStats Player => GameContext.Instance.Player;
  public Enemy Enemy;
  public int Damage;
  public IStatusEffect StatusEffect;
  public DamageType DamageType;

  public ResolveEffectHitContext(Enemy enemy, int baseDamage, IStatusEffect statusEffect, DamageType damageType)
  {
    Enemy = enemy;
    Damage = baseDamage;
    StatusEffect = statusEffect;
    DamageType = damageType;
  }
}

public enum DamageType
{
  Normal,
  Fire,
  Ice,
  Lightning,
  Laser,
  Missile,
  Explosion
}

public class CombatResolver : Singleton<CombatResolver>
{
  public HitResult ResolveBallHit(ResolveBallHitContext ctx)
  {
    // 1) compute damage (from player stats, ball stats, skills, etc.)
    int damage = ComputeDamage(ctx.Player.Stats.Attack, ctx.BallDamage);
    var dmgCtx = new DamageContext
    {
      amount = damage,
    };

    // 2) apply to health (keep health logic minimal & local)
    bool killed = ApplyDamage(ctx.Enemy, dmgCtx);

    // 3) apply OnHitEffect from ball
    if (ctx.OnHitEffect)
    {
      var effectCtx = new EffectContext(ctx.Enemy);
      ctx.OnHitEffect.Execute(effectCtx);

    }

    // 3.1) apply StatusEffect from hitbox (if any)

    // 4) update global combo (if you count each contact)
    GameContext.Instance.ComboManager.Increment(1);

    // 5) publish ONE rich event for reactions (skills, UI, sounds)
    var evt = new OnHitEvent(
        GameContext.Instance.Player,
        ctx.Enemy,
        damage,
        killed,
        ctx.DamageType

        );
    EventBus.Publish(evt);

    // if (killed)
    //   EventBus.Publish(new EnemyDeathEvent(ctx.enemy, ctx.hitPoint));

    return new HitResult { finalDamage = damage, killed = killed };
  }


  public HitResult ResolveEffectHit(ResolveEffectHitContext ctx)
  {
    // 1) compute damage (from player stats, ball stats, skills, etc.)
    int damage = ctx.Damage;
    var dmgCtx = new DamageContext
    {
      amount = damage,
      damageType = ctx.DamageType
    };

    // 2) apply to health (keep health logic minimal & local)
    bool killed = ApplyDamage(ctx.Enemy, dmgCtx);


    // 5) publish ONE rich event for reactions (skills, UI, sounds)
    var evt = new OnHitEvent(
        GameContext.Instance.Player,
        ctx.Enemy,
        damage,
        killed,
        ctx.DamageType

        );
    EventBus.Publish(evt);

    return new HitResult { finalDamage = damage, killed = killed };

  }

  private bool ApplyDamage(Enemy enemy, DamageContext dmgCtx)
  {
    if (dmgCtx.amount < 0) return false;
    else if (dmgCtx.amount == 0)
    {
      return false;
    }
    else
    {
      var damageText = "-" + GameUtils.FormatHealthText(dmgCtx.amount);
      SpawnDamagePopup(enemy.Position, damageText, dmgCtx.damageType);
    }
    var killed = enemy.HealthComponent.TakeDamage(dmgCtx);
    return killed;
  }

  public void PlayerTakeDamage(DamageContext dmgCtx)
  {
    if (dmgCtx.amount <= 0) return;

    var player = GameContext.Instance.Player;
    var damageText = "-" + GameUtils.FormatHealthText(dmgCtx.amount);
    SpawnDamagePopup(player.Position, damageText, dmgCtx.damageType);

    player.HealthComponent.TakeDamage(dmgCtx);
  }


  public void SpawnDamagePopup(Vector3 pos, string dmgTxt, DamageType dmgType)
  {
    var pool = GameContext.Instance.UIPool;
    var popup = pool.GetDamagePopup();
    popup.transform.SetParent(pool.transform, false);

    Color color = DamageVisualConfig.GetColor(dmgType);
    var damageText = dmgTxt;

    popup.GetComponent<DamagePopup>().Setup(pos, damageText, color, pool);
  }

  private int ComputeDamage(int playerAttack, int baseDamage)
  {
    var baseDmg = Mathf.RoundToInt(playerAttack * baseDamage);
    return baseDmg;
  }
}
