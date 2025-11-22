using System;
using System.Collections.Generic;
using UnityEngine;

public class CombatResolver : Singleton<CombatResolver>
{
  [SerializeField] private StatusEffectDatabase statusEffectDatabase;
  private readonly List<IDamageModifier> BallModifiers = new()
    {
      new PlayerAttackModifier(),
      new AttackModifier(),
      new DefenseModifier(),
      new HitDistanceModifier(),
      new HitDirectionModifier(),
    };
  private readonly List<IDamageModifier> EffectModifiers = new()
    {
      new PlayerAttackModifier(),
    };

  private readonly List<IDamageModifier> FixedModifiers = new()
  {
  };

  public HitResult ResolveHit(DamageContext ctx)
  {
    // Due to extremely fast pace, the enemy might be already dead
    if (!ctx.Enemy) return new HitResult { finalDamage = 0, killed = false };

    // 1) compute damage (from player stats, ball stats, skills, etc.)
    int damage = ComputeDamage(ctx);

    // 2) apply to health (keep health logic minimal & local)
    bool killed = ApplyDamage(ctx);

    // 3) apply OnHitEffect from ball
    if (ctx.OnHitEffect)
    {
      var effectCtx = new EffectContext(ctx.Enemy);
      ctx.OnHitEffect.Execute(effectCtx);

    }

    // 3.1) apply StatusEffect from hitbox (if any)
    if (!killed && ctx.StatusEffect != StatusEffectType.None)
    {
      var statusEffectConfig = statusEffectDatabase.GetConfig(ctx.StatusEffect);
      Debug.Log("Applying status effect: " + statusEffectConfig + ctx.StatusEffect);
      statusEffectConfig?.ApplyEffect(ctx.Enemy);
    }

    // 4) update global combo (if you count each contact)
    if (ctx.SourceType == DamageSourceType.Ball)
      LevelContext.Instance.ComboManager.Increment(1);

    // 5) publish ONE rich event for reactions (skills, UI, sounds)
    var evt = new OnHitEvent(
        LevelContext.Instance.Player,
        ctx.Enemy,
        damage,
        killed,
        ctx.DamageType

        );
    EventBus.Publish(evt);

    return new HitResult { finalDamage = damage, killed = killed };
  }


  private bool ApplyDamage(DamageContext ctx)
  {
    if (ctx.FinalDamage < 0) return false;
    else if (ctx.FinalDamage == 0)
    {
      return false;
    }
    else
    {
      var damageText = "-" + GameUtils.FormatHealthText(ctx.FinalDamage);
      SpawnDamagePopup(ctx.Enemy.Position, damageText, ctx.DamageType);
    }
    var killed = ctx.Enemy.HealthComponent.TakeDamage(ctx);
    return killed;
  }

  public void PlayerTakeDamage(PlayerDamageContext ctx)
  {
    if (ctx.FinalDamage <= 0) return;

    var player = LevelContext.Instance.Player;
    var damageText = "-" + GameUtils.FormatHealthText(ctx.FinalDamage);
    SpawnDamagePopup(player.Position, damageText);

    player.HealthComponent.PlayerTakeDamage(ctx);
  }


  public void SpawnDamagePopup(Vector3 pos, string dmgTxt, DamageType dmgType = DamageType.Normal)
  {
    var pool = LevelContext.Instance.UIPool;
    var popup = pool.GetDamagePopup();
    popup.transform.SetParent(pool.transform, false);

    Color color = DamageVisualConfig.GetColor(dmgType);
    var damageText = dmgTxt;

    popup.GetComponent<DamagePopup>().Setup(pos, damageText, color, pool);
  }

  private int ComputeDamage(DamageContext ctx)
  {
    List<IDamageModifier> globalModifiers =
        ctx.SourceType switch
        {
          DamageSourceType.Ball => BallModifiers,
          DamageSourceType.Effect => EffectModifiers,
          DamageSourceType.Fixed => FixedModifiers,
          _ => throw new NotImplementedException()
        };

    foreach (var m in globalModifiers)
    {
      m.Apply(ctx);
      if (ctx.IsBlocked) break;
    }

    return ctx.FinalDamage;
  }

}
