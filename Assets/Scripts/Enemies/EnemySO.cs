using UnityEngine;

public enum EnemyVariant
{
    Normal,
    Boss
}

public enum EnemySpecie
{
    Skeleton,
    PeaShooter,
    Slime,
    Bomber,
    Shielder,
    Spider,
    Summoner,
    Poisoner,
    Healer

}

[CreateAssetMenu(menuName = "MiniPunBall/Board/EnemySO")]
public class EnemySO : ScriptableObject, IBoardData
{
    public string Id => name;

    [Header("Basic Info")]
    public string Name;
    public EnemyVariant Variant;
    public EnemySpecie Specie;
    public Sprite Icon;

    [TextArea] public string Description;

    [Header("Combat Stats")]
    public int baseHealth = 100;
    public int baseAttack = 10;

    [Header("Behaviors")]
    public DeathEffect deathEffect;
    public EnemySkillBehavior enemySkillBehavior;

}
