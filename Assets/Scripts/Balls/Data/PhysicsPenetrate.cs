using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MiniPunBall/Ball/PhysicsPenetrate")]
public class PhysicsPenetrate : BallPhysicsSO
{
  public override IBallPhysicsBehavior CreateBehaviorInstance()
      => new PhysicsPenetrateRuntime();
}

public class PhysicsPenetrateRuntime : IBallPhysicsBehavior
{
  private BallPhysics physics;
  private BallBase ball;

  // Tracks which enemies were hit in the CURRENT segment
  private readonly HashSet<Enemy> hitEnemies = new HashSet<Enemy>();

  public void Init(BallPhysics physics, BallBase ballBase)
  {
    this.physics = physics;
    this.ball = ballBase;
    hitEnemies.Clear();
  }

  public void Move(float deltaTime)
  {
    physics.StandardMove(deltaTime);
  }

  public void OnHitEnemy(RaycastHit hit)
  {
    var hitbox = hit.collider.gameObject.GetComponent<Hitbox>();
    if (hitbox != null)
    {
      Enemy enemy = hitbox.Enemy;
      if (!hitEnemies.Contains(enemy))
      {
        hitEnemies.Add(enemy);
        physics.ProcessDamage(hit);
      }
      // else: already hit, do nothing
    }
  }

  public void OnHitWall(RaycastHit hit)
  {
    physics.Reflect(hit);

    Reset();
  }

  public void Reset()
  {
    hitEnemies.Clear();
  }
}