using UnityEngine;

[CreateAssetMenu(menuName = "MiniPunBall/Ball/BallSO")]
public class BallSO : ScriptableObject
{
    public BallType BallType;
    public float BaseDamage;
    public DamageType DamageType;

    [TextArea] public string Description;
    public GameObject BallPrefab;
    public EffectSO<EffectContext> OnHitEffect;
    public StatusEffectSO StatusEffect;
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
    SplitMini,
    Drill,
    Bomb
}
