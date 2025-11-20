using UnityEngine;

[CreateAssetMenu(menuName = "MiniPunBall/Ball/SplitPhysicsRuntimeSO")]
public class SplitPhysicsRuntimeSO : BallPhysicsSO
{
  public override IBallPhysicsBehavior CreateBehaviorInstance()
      => new SplitPhysicsRuntime();
}

public class SplitPhysicsRuntime : IBallPhysicsBehavior
{
  private BallPhysics physics;
  private BallBase ball;

  public void Init(BallPhysics physics, BallBase ballBase)
  {
    this.physics = physics;
    this.ball = ballBase;
  }

  public void Move(float deltaTime)
  {
    physics.StandardMove(deltaTime);
  }

  public void OnHitEnemy(RaycastHit hit)
  {
    // physics.ProcessDamage(hit);
    // physics.SpawnSplitBalls(hit.point);
    // physics.HandleBallReturned(ball);
  }
}

