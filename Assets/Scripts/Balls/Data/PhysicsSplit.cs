using UnityEngine;

[CreateAssetMenu(menuName = "MiniPunBall/Ball/PhysicsSplit")]
public class PhysicsSplit : BallPhysicsSO
{
  public override IBallPhysicsBehavior CreateBehaviorInstance()
      => new PhysicsSplitRuntime();
}

public class PhysicsSplitRuntime : IBallPhysicsBehavior
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

    Vector3 reflectedDirection = physics.CalculateReflection(hit.normal);
    Split(BallType.SplitMini, hit.point, reflectedDirection);

    physics.HandleBallReturned(ball);
  }


  private void Split(BallType ballType, Vector3 splitPoint, Vector3 originalDirection)
  {
    int splitCount = 2; // Number of split balls
    float angleOffset = 30f; // Total angle spread between the two balls

    for (int i = 0; i < splitCount; i++)
    {
      // Calculate split direction: one ball goes left, one goes right
      float angle = (i == 0) ? -angleOffset / 2f : angleOffset / 2f;
      Vector3 splitDirection = Quaternion.Euler(0, angle, 0) * originalDirection;

      LevelContext.Instance.Player.Balls.SpawnEphemeralBall(ballType, 1, splitPoint, splitDirection);
    }
  }
}

