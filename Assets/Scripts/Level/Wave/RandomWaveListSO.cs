using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RandomWaveListSO", menuName = "MiniPunBall/RandomWaveListSO", order = 0)]
public class RandomWaveListSO : WaveListSO
{
    [SerializeField] private Enemy[] availableEnemies;
    // [SerializeField] private GameObject bossPrefab;
    [SerializeField] private BoardObject ballPickup;
    [SerializeField] private BoardObject boxPickup;
    [SerializeField] LevelMultiplierSO levelMultiplier;
    [SerializeField] int totalWaves = 20;


    [SerializeField] private List<int> bossWaveIndices = new() { 20 }; // e.g., {20, 40} for 40-wave level

    public override WaveContent GenerateWave(int level, int waveNumber)
    {
        var content = new WaveContent
        {
            levelMultiplier = levelMultiplier,
            LevelNumber = level,
            WaveNumber = waveNumber
        };

        if (waveNumber > totalWaves)
        {
            content.waveRows = new WaveRow[0];
            return content;
        }

        // Before boss wave: empty wave
        foreach (var bossIndex in bossWaveIndices)
        {
            if (waveNumber == bossIndex - 1)
            {
                content.waveRows = new WaveRow[0];
                return content;
            }
        }

        List<WaveRow> rows = new List<WaveRow>();

        // Full row: enemies + correct pickup
        void AddFullRow(int rowIndex)
        {
            var row = new WaveRow();
            row.index = rowIndex;
            int enemyCount = Random.Range(1, 6);
            for (int i = 0; i < enemyCount; i++)
            {
                var enemy = availableEnemies[Random.Range(0, availableEnemies.Length)];
                row.boardObjects.Add(enemy);
            }
            // ballPickup on even waves (including wave 1), boxPickup on odd waves
            if (waveNumber % 2 == 0 || waveNumber == 1)
                row.boardObjects.Add(ballPickup);
            else
                row.boardObjects.Add(boxPickup);
            rows.Add(row);
        }

        // Enemy-only row
        void AddEnemyRow(int rowIndex, int count = -1)
        {
            var row = new WaveRow();
            row.index = rowIndex;
            int enemyCount = count > 0 ? count : Random.Range(1, 6);
            for (int i = 0; i < enemyCount; i++)
            {
                var enemy = availableEnemies[Random.Range(0, availableEnemies.Length)];
                row.boardObjects.Add(enemy);
            }
            rows.Add(row);
        }

        if (waveNumber == 1)
        {
            // Row 6: full row (enemies + ballPickup)
            AddFullRow(6);
            // Row 5: 2 enemies only
            AddEnemyRow(5, 2);
            // Row 4: 2 enemies only
            AddEnemyRow(4, 2);
        }
        else
        {
            // Row 6: full row (enemies + ballPickup) on even, (enemies + boxPickup) on odd
            AddFullRow(6);
        }

        content.waveRows = rows.ToArray();
        return content;
    }

}

[System.Serializable]
public class WaveContent
{
    public WaveRow[] waveRows;
    public LevelMultiplierSO levelMultiplier;
    public int LevelNumber;
    public int WaveNumber;
    public float HPMultiplier => levelMultiplier != null ? levelMultiplier.GetWaveHpMultiplier(LevelNumber, WaveNumber) : 1f;
    public float AttackMultiplier => levelMultiplier != null ? levelMultiplier.GetWaveAttackMultiplier(LevelNumber) : 1f;
}

[System.Serializable]
public class WaveRow
{
    public int index;
    public List<BoardObject> boardObjects = new();
}

