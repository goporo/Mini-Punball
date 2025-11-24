using UnityEngine;

[CreateAssetMenu(menuName = "MiniPunBall/Ball/PhysicsDestruct")]
public class PhysicsDestruct : BallPhysicsSO
{
  public override IBallPhysicsBehavior CreateBehaviorInstance()
      => new PhysicsDestructRuntime();
}

public class PhysicsDestructRuntime : IBallPhysicsBehavior
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
    var hitbox = hit.collider.gameObject.GetComponent<Hitbox>();
    EventBus.Publish(new OnVoidHitEvent(new EffectContext(hitbox.Enemy)));

    physics.ProcessDamage(hit);
    LevelContext.Instance.VFXManager.SpawnVFX<VFXVoid, BasicVFXParams>(
      new BasicVFXParams
      {
        Position = hit.point,
      }
    );
    physics.HandleBallReturned(ball);
  }

  public void OnHitWall(RaycastHit hit)
  {
    physics.Reflect(hit);
  }
}
