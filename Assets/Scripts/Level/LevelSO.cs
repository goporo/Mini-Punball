using UnityEngine;

[CreateAssetMenu(fileName = "LevelSO", menuName = "MiniPunBall/LevelSO", order = 0)]
public class LevelSO : ScriptableObject
{
    public GameObject boardPrefab;
    public EnemySO[] availableEnemies; // e.g., goblin, orc, archer
    [HideInInspector] public int spawnLine = 1; // z-axis position where enemies spawn

    public int totalWaves = 20;
    public LevelMultipliers levelMultipliers;
    public WaveListSO waveListSO;

}

[System.Serializable]
public class LevelMultipliers
{
    public float hpMultiplier = 1.0f;
    public float attackMultiplier = 1.0f;
}
