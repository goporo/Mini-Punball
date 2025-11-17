using UnityEngine;

[CreateAssetMenu(fileName = "BallSO", menuName = "MiniPunBall/BallSO", order = 0)]
public class BallSO : ScriptableObject
{
    public BallType BallType;
    public int BaseDamage;
    public IStatusEffect StatusEffect;
    public DamageType DamageType;

    [TextArea] public string Description;
    public GameObject BallPrefab;
    public EffectSO<EffectContext> OnHitEffect;

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
    Stab
}
