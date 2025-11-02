using UnityEngine;

[CreateAssetMenu(fileName = "BallSO", menuName = "MiniPunBall/BallSO", order = 0)]
public class BallSO : ScriptableObject
{
    public BallType BallType;
    public int BaseDamage;
    public IStatusEffect statusEffect;
    [TextArea] public string Description;
    public GameObject BallPrefab;
    public EffectSO<BallHitContext> onHitEffect;

}

public enum BallType
{
    Normal,
    Fire,
    Ice,
    Lightning,
    Missile,
    Laser
}
