using UnityEngine;

[CreateAssetMenu(fileName = "BallSO", menuName = "MiniPunBall/Ball/BallSO", order = 0)]
public class BallSO : ScriptableObject
{
    public BallType BallType;
    public float BaseDamage;
    public IStatusEffect StatusEffect;
    public DamageType DamageType;

    [TextArea] public string Description;
    public GameObject BallPrefab;
    public EffectSO<EffectContext> OnHitEffect;
    public BallPhysicsSO PhysicsBehavior;

}

public enum BallType
{
    Normal,
    Fire,
    Ice,
    Lightning,
    Missile,
    HLaser,
    VLaser,
    Combat,
    Sniper,
    Stab,
    Void,
    Split,
    Drill,
    Bomb
}
