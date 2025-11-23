using UnityEngine;

public interface ICastOrigin
{
  void OnSpawn(EffectSO<EffectCastContext> ctx);   // optional hook
}

public static class CastOriginFactory
{
  public static ICastOrigin GetCastInstance(ECastOrigin origin)
  {
    return origin switch
    {
      ECastOrigin.Enemy => new EnemyOrigin(),
      ECastOrigin.Combo => new ComboOrigin(),
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

    float adjustedZ = target.Position.z + (6 - target.CurrentCell.y);
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

public enum ECastOrigin
{
  Enemy,
  Combo,
  Clone
}