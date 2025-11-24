using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MiniPunBall/Skill/EffSpawnLaser")]
public class EffSpawnLaser : EffectSO<EffectCastContext>
{
  public int multiplier = 1;
  [SerializeField] private Direction direction = Direction.Horizontal;

  public override void Execute(EffectCastContext ctx)
  {
    var originInstance = CastOriginFactory.GetCastInstance(ctx.CastSource);
    var enemy = ctx.CastEnemy;
    if (enemy == null)
    {
      enemy = LevelContext.Instance.BoardState.GetRandomEnemy();
      if (enemy == null) return;
    }
    var spawnPos = CastOriginFactory.GetGroundOrigin(enemy);


    if (direction == Direction.Horizontal)
    {
      SpawnAndApplyLaser(
        spawnPos - 15f * Vector3.right,
        spawnPos + 15f * Vector3.right,
        LevelContext.Instance.BoardState.GetRowObjects(enemy.CurrentCell));
    }
    else if (direction == Direction.Vertical)
    {
      SpawnAndApplyLaser(
        spawnPos - 15f * Vector3.forward,
        spawnPos + 15f * Vector3.forward,
        LevelContext.Instance.BoardState.GetColumnObjects(enemy.CurrentCell));
    }
    else // Both
    {
      // Horizontal
      SpawnAndApplyLaser(
        spawnPos - 15f * Vector3.right,
        spawnPos + 15f * Vector3.right,
        LevelContext.Instance.BoardState.GetRowObjects(enemy.CurrentCell));

      // Vertical
      SpawnAndApplyLaser(
        spawnPos - 15f * Vector3.forward,
        spawnPos + 15f * Vector3.forward,
        LevelContext.Instance.BoardState.GetColumnObjects(enemy.CurrentCell, enemy));
    }
    originInstance.OnSpawn(this);

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


