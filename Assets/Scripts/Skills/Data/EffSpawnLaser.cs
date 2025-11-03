using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MiniPunBall/Skill/EffSpawnLaser", order = 0)]
public class EffSpawnLaser : EffectSO<BallHitContext>
{
  public int multiplier = 1;
  [SerializeField] private Direction direction = Direction.Horizontal;
  [SerializeField] private GameObject laserPrefab;
  public override void Execute(BallHitContext ctx)
  {
    var laser = Instantiate(laserPrefab, ctx.Enemy.Position, Quaternion.identity).GetComponent<Laser>();
    if (direction == Direction.Horizontal)
    {
      laser.Init(new Vector3(ctx.Enemy.Position.x - 10f, 1f, ctx.Enemy.Position.z), new Vector3(ctx.Enemy.Position.x + 10f, 1f, ctx.Enemy.Position.z));
      var targets = GameContext.Instance.BoardState.GetRowObjects(ctx.Enemy.CurrentCell);
      ApplyDamageToSelectedTargets(ctx, targets);
    }
    else if (direction == Direction.Vertical)
    {
      laser.Init(new Vector3(ctx.Enemy.Position.x, 1f, ctx.Enemy.Position.z - 10f), new Vector3(ctx.Enemy.Position.x, 1f, ctx.Enemy.Position.z + 10f));
      var targets = GameContext.Instance.BoardState.GetColumnObjects(ctx.Enemy.CurrentCell);
      ApplyDamageToSelectedTargets(ctx, targets);
    }

  }

  private void ApplyDamageToSelectedTargets(BallHitContext ctx, List<BoardObject> targets)
  {
    foreach (var obj in targets)
    {
      if (obj == null) continue;
      if (obj.TryGetComponent<IDamageable>(out var target))
      {
        var damageCtx = new DamageContext(ctx.Player.Stats.Attack * multiplier, null);
        target.TakeDamage(damageCtx);
      }
    }
  }


  public enum Direction
  {
    Horizontal,
    Vertical
  }
}


