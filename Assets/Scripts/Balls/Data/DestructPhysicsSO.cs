using UnityEngine;

[CreateAssetMenu(fileName = "DestructPhysicsSO", menuName = "MiniPunBall/Ball/DestructPhysicsSO", order = 0)]
public class DestructPhysicsSO : BallPhysicsSO
{
  public override IBallPhysicsBehavior CreateBehaviorInstance()
      => new DestructPhysicsRuntime();
}

public class DestructPhysicsRuntime : IBallPhysicsBehavior
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
}
