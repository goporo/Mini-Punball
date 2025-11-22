using UnityEngine;
using System;
using System.Collections.Generic;
using System.Reflection;

[CreateAssetMenu(menuName = "MiniPunBall/Skill/EffDropComet")]
public class EffDropComet : EffectSO<EffectCastContext>
{
  [SerializeField] private CometType cometType;
  [SerializeField] private int multiplier = 2;

  // Static mapping from enum to VFX type - initialize once
  private static readonly Dictionary<CometType, System.Type> CometTypeToVFX = new()
  {
    { CometType.Fire, typeof(VFXCometFire) },
    // { CometType.Ice, typeof(VFXCometIce) },
    // { CometType.Bomb, typeof(VFXCometBomb) },
    // { CometType.Void, typeof(VFXCometVoid) }
  };

  private static readonly Dictionary<CometType, DamageType> CometTypeToDamage = new()
  {
    { CometType.Fire, DamageType.Fire },
    { CometType.Ice, DamageType.Ice },
    { CometType.Bomb, DamageType.Explosion },
    { CometType.Void, DamageType.Void }
  };

  public override void Execute(EffectCastContext ctx)
  {
    var enemies = LevelContext.Instance.BoardState.GetRandomEnemies(1);
    if (enemies == null || enemies.Count == 0)
      return;

    if (!CometTypeToVFX.TryGetValue(cometType, out var vfxType))
    {
      Debug.LogError($"No VFX type mapped for comet type: {cometType}");
      return;
    }

    var target = enemies[0];
    var damageType = CometTypeToDamage[cometType];
    var dmgCtx = DamageContext.CreateEffectDamage(
      target,
      multiplier,
      damageType,
      StatusEffectType.Burn
    );

    SpawnCometVFX(vfxType, target, dmgCtx);
    if (ctx.IsOriginalCast)
      EventBus.Publish(new OnComboCastEvent(this));

  }

  private void SpawnCometVFX(System.Type vfxType, Enemy target, DamageContext dmgCtx)
  {
    var vfxManager = LevelContext.Instance.VFXManager;
    if (vfxManager == null)
      return;

    var spawnParams = new TargetVFXParams
    {
      Position = target.Position + Vector3.up * 5f,
      Target = target,
      Callback = () => CombatResolver.Instance.ResolveHit(dmgCtx)
    };

    // Use reflection to call SpawnVFX<T, TParams> with runtime type
    InvokeGenericSpawnVFX(vfxManager, vfxType, spawnParams);
  }

  private void InvokeGenericSpawnVFX(VFXManager manager, System.Type vfxType, TargetVFXParams spawnParams)
  {
    try
    {
      var method = typeof(VFXManager).GetMethod("SpawnVFX",
        BindingFlags.Public | BindingFlags.Instance);

      var genericMethod = method.MakeGenericMethod(vfxType, typeof(TargetVFXParams));
      genericMethod.Invoke(manager, new object[] { spawnParams });
    }
    catch (Exception ex)
    {
      Debug.LogError($"Failed to spawn VFX of type {vfxType.Name}: {ex.Message}");
    }
  }
}

public enum CometType
{
  Fire,
  Ice,
  Bomb,
  Void
}