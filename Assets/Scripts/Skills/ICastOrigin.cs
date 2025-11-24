using UnityEngine;

public interface ICastOrigin
{
  public void OnSpawn(EffectSO<EffectCastContext> ctx);   // optional hook

}

public static class CastOriginFactory
{
  public static ICastOrigin GetCastInstance(ECastSource origin)
  {
    return origin switch
    {
      ECastSource.Enemy => new EnemyOrigin(),
      ECastSource.Combo => new ComboOrigin(),
      ECastSource.Effect => new EffectOrigin(),
      _ => new EnemyOrigin(),
    };
  }

  public static Vector3 GetGroundOrigin(Enemy target)
  {
    if (target == null)
    {
      Debug.LogWarning("EnemyOrigin: target is null");
      return Vector3.zero;
    }
    return new Vector3(target.Position.x, 1f, target.Position.z);
  }

  public static Vector3 GetSkyOrigin(Enemy target)
  {
    if (target == null)
    {
      Debug.LogWarning("EnemyOrigin: target is null");
      return Vector3.zero;
    }

    float adjustedZ = target.Position.z + (6 - target.CurrentCell.y) + 3; // Adjust Z so it spawns from top screen
    return new Vector3(target.Position.x, 5f, adjustedZ);
  }
}

public class EnemyOrigin : ICastOrigin
{
  public void OnSpawn(EffectSO<EffectCastContext> ctx) { }
}

public class ComboOrigin : ICastOrigin
{
  public void OnSpawn(EffectSO<EffectCastContext> eff)
  {
    EventBus.Publish(new OnComboCastEvent(eff));
  }
}

public class EffectOrigin : ICastOrigin
{
  public void OnSpawn(EffectSO<EffectCastContext> ctx) { }
}

public enum ECastSource
{
  Enemy, // effect spawned from an enemy
  Combo, // effect spawned from a combo
  Effect // effect spawned from another effect
}