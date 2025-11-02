using UnityEngine;

public enum EnemyVariant
{
    Normal,
    Boss
}

public enum EnemySpecie
{
    Skeleton,

}

[CreateAssetMenu(fileName = "EnemySO", menuName = "MiniPunBall/Board/EnemySO", order = 0)]
public class EnemySO : ScriptableObject, IBoardData
{
    public string Id => name;

    [Header("Basic Info")]
    public string enemyName;
    public EnemyVariant variant;
    public EnemySpecie specie;

    [TextArea] public string Description;

    [Header("Combat Stats")]
    public int baseHealth = 100;
    public int baseAttack = 10;

    [Header("Behaviors")]
    public MoveBehavior moveBehavior;
    public DeathEffect deathEffect;
    public EnemySkillBehavior enemySkillBehavior;

}
