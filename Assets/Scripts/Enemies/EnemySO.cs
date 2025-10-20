using UnityEngine;

public enum EnemyType
{
    Normal,
    Boss
}

[CreateAssetMenu(fileName = "EnemySO", menuName = "MiniPunBall/EnemySO", order = 0)]
public class EnemySO : ScriptableObject
{
    [Header("Basic Info")]
    public string enemyName;
    public EnemyType type;
    public Color color = Color.white;

    [Header("Combat Stats")]
    public float baseHealth = 100f;
    public float baseAttack = 10f;
    public float moveSpeed = 2f;

    [Header("Rewards")]
    public int xpReward = 10;
    public int goldReward = 5;

    [Header("Visuals / Prefab")]
    public GameObject enemyPrefab;
}
