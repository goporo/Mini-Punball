using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MiniPunBall/Skill/EffSpawnLaser")]
public class EffSpawnLaser : EffectSO<EffectCastContext>
{
  public int multiplier = 1;
  [SerializeField] private Direction direction = Direction.Horizontal;
  private ICastOrigin origin;

  public override void Execute(EffectCastContext ctx)
  {
    origin = CastOriginFactory.GetCastInstance(ctx.CastOrigin);
    var target = ctx.Target;
    if (target == null)
    {
      target = LevelContext.Instance.BoardState.GetRandomEnemy();
      if (target == null) return;
    }
    var spawnPos = CastOriginFactory.GetGroundOrigin(target);


    if (direction == Direction.Horizontal)
    {
      SpawnAndApplyLaser(
        spawnPos - 15f * Vector3.right,
        spawnPos + 15f * Vector3.right,
        LevelContext.Instance.BoardState.GetRowObjects(target.CurrentCell));
    }
    else if (direction == Direction.Vertical)
    {
      SpawnAndApplyLaser(
        spawnPos - 15f * Vector3.forward,
        spawnPos + 15f * Vector3.forward,
        LevelContext.Instance.BoardState.GetColumnObjects(target.CurrentCell));
    }
    else // Both
    {
      // Horizontal
      SpawnAndApplyLaser(
        spawnPos - 15f * Vector3.right,
        spawnPos + 15f * Vector3.right,
        LevelContext.Instance.BoardState.GetRowObjects(target.CurrentCell));

      // Vertical
      SpawnAndApplyLaser(
        spawnPos - 15f * Vector3.forward,
        spawnPos + 15f * Vector3.forward,
        LevelContext.Instance.BoardState.GetColumnObjects(target.CurrentCell, target));
    }
    origin.OnSpawn(this);

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


