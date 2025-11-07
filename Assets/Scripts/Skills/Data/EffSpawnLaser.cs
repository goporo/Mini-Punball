using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MiniPunBall/Skill/EffSpawnLaser", order = 0)]
public class EffSpawnLaser : EffectSO<EffectContext>
{
  public int multiplier = 1;
  [SerializeField] private Direction direction = Direction.Horizontal;

  public override void Execute(EffectContext ctx)
  {
    Vector3 startPoint;
    Vector3 endPoint;
    List<BoardObject> targets;

    if (direction == Direction.Horizontal)
    {
      startPoint = new Vector3(ctx.Enemy.Position.x - 10f, 1f, ctx.Enemy.Position.z);
      endPoint = new Vector3(ctx.Enemy.Position.x + 10f, 1f, ctx.Enemy.Position.z);
      targets = GameContext.Instance.BoardState.GetRowObjects(ctx.Enemy.CurrentCell);
    }
    else // Vertical
    {
      startPoint = new Vector3(ctx.Enemy.Position.x, 1f, ctx.Enemy.Position.z - 10f);
      endPoint = new Vector3(ctx.Enemy.Position.x, 1f, ctx.Enemy.Position.z + 10f);
      targets = GameContext.Instance.BoardState.GetColumnObjects(ctx.Enemy.CurrentCell);
    }

    // Spawn laser VFX from centralized VFXManager pool (generic)
    GameContext.Instance.VFXManager.SpawnVFX<LaserVFX, LaserVFXParams>(
      new LaserVFXParams
      {
        StartPoint = startPoint,
        EndPoint = endPoint,
      }
    );

    // Apply damage to targets
    ApplyDamageToSelectedTargets(ctx, targets);
  }

  private void ApplyDamageToSelectedTargets(EffectContext ctx, List<BoardObject> targets)
  {
    foreach (var obj in targets)
    {
      if (obj == null) continue;
      if (obj.TryGetComponent<Enemy>(out var target))
      {
        CombatResolver.Instance.ResolveEffectHit(
          new ResolveEffectHitContext(target, ctx.Player.Stats.Attack * multiplier, null, DamageType.Laser)
        );
      }
      else if (obj.TryGetComponent<IDamageable>(out var damageable))
      {
        damageable.TakeDamage(new DamageContext { });
      }
    }
  }

  public enum Direction
  {
    Horizontal,
    Vertical
  }
}


