using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "LevelMultiplierSO", menuName = "MiniPunBall/LevelMultiplierSO", order = 0)]
public class LevelMultiplierSO : ScriptableObject
{
    public WaveMultiplier[] waveMultipliers;
    public LevelMultiplier[] levelMultipliers;

    public float GetWaveHpMultiplier(int levelNumber, int waveNumber)
    {
        float levelFactor = levelMultipliers.FirstOrDefault(l => l.levelNumber == levelNumber)?.hpMultiplier ?? 1f;
        float waveFactor = waveMultipliers.FirstOrDefault(w => w.waveNumber == waveNumber)?.hpMultiplier ?? 1f;
        return levelFactor * waveFactor;
    }

    public float GetWaveAttackMultiplier(int levelNumber)
    {
        return levelMultipliers.FirstOrDefault(l => l.levelNumber == levelNumber)?.attackMultiplier ?? 1f;
    }

}

[System.Serializable]
public class WaveMultiplier
{
    public int waveNumber;
    public float hpMultiplier = 1.0f;
    // Attack fixed every wave
}

[System.Serializable]
public class LevelMultiplier
{
    public int levelNumber;
    public float hpMultiplier = 1.0f;
    public float attackMultiplier = 1.0f;
}

