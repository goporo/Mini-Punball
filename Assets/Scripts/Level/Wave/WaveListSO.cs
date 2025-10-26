using UnityEngine;

public abstract class WaveListSO : ScriptableObject
{
    public abstract WaveContent GenerateWave(int waveNumber);
}