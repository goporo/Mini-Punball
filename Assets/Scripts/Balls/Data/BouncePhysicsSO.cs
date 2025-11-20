using UnityEngine;

[CreateAssetMenu(fileName = "BouncePhysicsSO", menuName = "MiniPunBall/Ball/BouncePhysicsSO", order = 0)]
public class BouncePhysicsSO : BallPhysicsSO
{
  public override IBallPhysicsBehavior CreateBehaviorInstance()
      => new BouncePhysicsRuntime();
}

public class BouncePhysicsRuntime : IBallPhysicsBehavior
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
    physics.ProcessDamage(hit);
    physics.Reflect(hit);
  }
}