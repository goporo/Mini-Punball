using UnityEngine;

[CreateAssetMenu(menuName = "MiniPunBall/Ball/BallSO")]
public class BallSO : ScriptableObject
{
    public BallType BallType;
    public float BaseDamage;
    public DamageType DamageType;

    [TextArea] public string Description;
    public GameObject BallPrefab;
    public EffectSO<EffectCastContext> OnHitEffect;
    public StatusEffectSO StatusEffect;
    public BallPhysicsSO PhysicsBehavior;
    public DamageModifierSO[] DamageModifiers;

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
    Bomb,
    InstantKill
}
