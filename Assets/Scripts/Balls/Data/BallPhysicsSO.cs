using UnityEngine;

public abstract class BallPhysicsSO : ScriptableObject
{
    /// <summary>
    /// Creates a runtime instance of this behavior.
    /// </summary>
    public abstract IBallPhysicsBehavior CreateBehaviorInstance();
}

public interface IBallPhysicsBehavior
{
    void Init(BallPhysics physics, BallBase ballBase);
    void Move(float deltaTime);
    void OnHitEnemy(RaycastHit hit);
}
