using System;
using UnityEngine;


[Serializable]
public struct BossList
{
    public int spawnWave;
    public Enemy bossEnemy;
}
[CreateAssetMenu(fileName = "LevelSO", menuName = "MiniPunBall/LevelSO", order = 0)]
public class LevelSO : ScriptableObject
{
    public int LevelNumber;
    public GameObject boardPrefab;
    public Enemy[] availableEnemies; // e.g., goblin, orc, archer

    [SerializeField] private BossList[] bossLists;
    [SerializeField] private int totalWaves;
    public BossList[] BossLists => bossLists;
    public int TotalWaves => totalWaves;
    public WaveListSO waveListSO;

    private void OnValidate()
    {
        if (availableEnemies == null)
        {
            availableEnemies = new Enemy[3];
        }
        else if (availableEnemies.Length != 3)
        {
            Enemy[] newArr = new Enemy[3];
            for (int i = 0; i < 3; i++)
            {
                if (i < availableEnemies.Length)
                {
                    newArr[i] = availableEnemies[i];
                }
                else
                {
                    // Copy previous if possible, else null
                    newArr[i] = i > 0 ? newArr[i - 1] : null;
                }
            }
            availableEnemies = newArr;
        }
    }

}
