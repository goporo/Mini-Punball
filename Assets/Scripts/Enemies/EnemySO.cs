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

[CreateAssetMenu(fileName = "EnemySO", menuName = "MiniPunBall/EnemySO", order = 0)]
public class EnemySO : ScriptableObject
{
    [Header("Basic Info")]
    public string enemyName;
    public EnemyVariant variant;
    public EnemySpecie specie;
    [TextArea] public string Description;

    [Header("Combat Stats")]
    public int baseHealth = 100;
    public int baseAttack = 10;

    [Header("Visuals / Prefab")]
    public GameObject enemyPrefab;
    [Header("Behaviors")]
    public MoveBehavior moveBehavior;
    public DeathBehavior deathBehavior;

}
