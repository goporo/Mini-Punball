using UnityEngine;

[CreateAssetMenu(fileName = "LevelSO", menuName = "MiniPunBall/LevelSO", order = 0)]
public class LevelSO : ScriptableObject
{
    public int LevelNumber;
    public GameObject boardPrefab;
    public EnemySO[] availableEnemies; // e.g., goblin, orc, archer
    [HideInInspector] public int spawnLine = 1; // z-axis position where enemies spawn

    public int totalWaves = 20;
    public WaveListSO waveListSO;

}
