using UnityEngine;

[CreateAssetMenu(menuName = "MiniPunBall/Ball/PhysicsBounce")]
public class PhysicsBounce : BallPhysicsSO
{
  public override IBallPhysicsBehavior CreateBehaviorInstance()
      => new PhysicsBounceRuntime();
}

public class PhysicsBounceRuntime : IBallPhysicsBehavior
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

  public void OnHitWall(RaycastHit hit)
  {
    physics.Reflect(hit);
  }
}