using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MiniPunBall/Skill/EffSpawnLaser")]
public class EffSpawnLaser : EffectSO<EffectContext>
{
  public int multiplier = 1;
  [SerializeField] private Direction direction = Direction.Horizontal;

  public override void Execute(EffectContext ctx)
  {
    if (direction == Direction.Horizontal)
    {
      SpawnAndApplyLaser(
        new Vector3(ctx.Enemy.Position.x - 15f, 1f, ctx.Enemy.Position.z),
        new Vector3(ctx.Enemy.Position.x + 15f, 1f, ctx.Enemy.Position.z),
        LevelContext.Instance.BoardState.GetRowObjects(ctx.Enemy.CurrentCell, ctx.Enemy));
    }
    else if (direction == Direction.Vertical)
    {
      SpawnAndApplyLaser(
        new Vector3(ctx.Enemy.Position.x, 1f, ctx.Enemy.Position.z - 15f),
        new Vector3(ctx.Enemy.Position.x, 1f, ctx.Enemy.Position.z + 15f),
        LevelContext.Instance.BoardState.GetColumnObjects(ctx.Enemy.CurrentCell, ctx.Enemy));
    }
    else // Both
    {
      // Horizontal
      SpawnAndApplyLaser(
        new Vector3(ctx.Enemy.Position.x - 15f, 1f, ctx.Enemy.Position.z),
        new Vector3(ctx.Enemy.Position.x + 15f, 1f, ctx.Enemy.Position.z),
        LevelContext.Instance.BoardState.GetRowObjects(ctx.Enemy.CurrentCell, ctx.Enemy));

      // Vertical
      SpawnAndApplyLaser(
        new Vector3(ctx.Enemy.Position.x, 1f, ctx.Enemy.Position.z - 15f),
        new Vector3(ctx.Enemy.Position.x, 1f, ctx.Enemy.Position.z + 15f),
        LevelContext.Instance.BoardState.GetColumnObjects(ctx.Enemy.CurrentCell, ctx.Enemy));
    }

  }

  private void SpawnAndApplyLaser(Vector3 startPoint, Vector3 endPoint, List<BoardObject> targets)
  {
    LevelContext.Instance.VFXManager.SpawnVFX<VFXLaser, LaserVFXParams>(
      new LaserVFXParams
      {
        StartPoint = startPoint,
        EndPoint = endPoint,
      }
    );
    ApplyDamageToSelectedTargets(targets);
  }

  private void ApplyDamageToSelectedTargets(List<BoardObject> targets)
  {
    foreach (var obj in targets)
    {
      if (obj == null) continue;
      if (obj.TryGetComponent<Enemy>(out var target))
      {
        var dmgCtx = DamageContext.CreateEffectDamage(
            target,
            multiplier,
            DamageType.Laser
        );
        CombatResolver.Instance.ResolveHit(dmgCtx);
      }
      else if (obj.TryGetComponent<IDamageable>(out var damageable))
      {
        var dmgCtx = DamageContext.CreateObstacleDamage();
        damageable.TakeDamage(dmgCtx);
      }
    }
  }

  public void SetDirection(Direction dir)
  {
    direction = dir;
  }

  public enum Direction
  {
    Horizontal,
    Vertical,
    Both
  }
}


