using UnityEngine;

public abstract class WaveListSO : ScriptableObject
{
    public abstract WaveContent GenerateWave(int level, int waveNumber, Enemy[] availableEnemies, BossList[] bossLists, int totalWaves);
}