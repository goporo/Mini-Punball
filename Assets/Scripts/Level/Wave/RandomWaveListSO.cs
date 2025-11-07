using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RandomWaveListSO", menuName = "MiniPunBall/RandomWaveListSO", order = 0)]
public class RandomWaveListSO : WaveListSO
{
    [SerializeField] private BoardObject ballPickup;
    [SerializeField] private BoardObject boxPickup;
    [SerializeField] private Enemy baseEnemy;
    [SerializeField] LevelMultiplierSO levelMultiplier;
    [SerializeField] int totalWaves = 20;



    [SerializeField] private List<int> bossWaveIndices = new() { 20 }; // e.g., {20, 40} for 40-wave level


    // Helper: choose an enemy from `enemies` using dynamic ratios based on array length
    // Ratios: 2 enemies -> 8:2, 3 enemies -> 6:2:2, 4 enemies -> 4:2:2:2
    private Enemy GetEnemyByRatio(Enemy[] enemies)
    {
        if (enemies == null || enemies.Length == 0)
            return null;

        int count = enemies.Length;

        int[] weights;
        if (count == 2)
            weights = new int[] { 8, 2 };
        else if (count == 3)
            weights = new int[] { 6, 2, 2 };
        else if (count >= 4)
            weights = new int[] { 4, 2, 2, 2 };
        else
            return enemies[0];

        int total = 0;
        for (int i = 0; i < Mathf.Min(count, weights.Length); i++)
            total += weights[i];

        int r = Random.Range(0, total);
        int acc = 0;
        for (int i = 0; i < Mathf.Min(count, weights.Length); i++)
        {
            acc += weights[i];
            if (r < acc)
                return enemies[i];
        }

        return enemies[count - 1];
    }

    public override WaveContent GenerateWave(int level, int wave, Enemy[] levelEnemies)
    {
        var content = new WaveContent
        {
            levelMultiplier = levelMultiplier,
            LevelNumber = level,
            WaveNumber = wave
        };

        if (wave > totalWaves)
        {
            content.waveRows = new WaveRow[0];
            return content;
        }

        // Before boss wave: empty wave
        foreach (var bossIndex in bossWaveIndices)
        {
            if (wave == bossIndex - 1)
            {
                content.waveRows = new WaveRow[0];
                return content;
            }
        }

        List<WaveRow> rows = new List<WaveRow>();

        // Build availableEnemies for this wave
        var availableEnemies = new List<Enemy>();

        // Always add base enemy first
        if (baseEnemy != null)
            availableEnemies.Add(baseEnemy);

        // Add enemies based on level
        if (levelEnemies != null && levelEnemies.Length >= 1)
        {
            if (wave >= 1)
            {
                // Wave 1+: add enemies[0]
                availableEnemies.Add(levelEnemies[0]);
            }

            if (wave >= 6 && levelEnemies.Length >= 2)
            {
                // Wave 6+: add enemies[1]
                availableEnemies.Add(levelEnemies[1]);
            }

            if (wave >= 12 && levelEnemies.Length >= 3)
            {
                // Wave 12+: add enemies[2]
                availableEnemies.Add(levelEnemies[2]);
            }
        }

        Enemy[] enemies = availableEnemies.ToArray();

        // Determine if this level introduces a new enemy type (must include in spawn)
        Enemy introducedEnemy = null;
        if (wave == 1 && levelEnemies != null && levelEnemies.Length >= 2)
            introducedEnemy = levelEnemies[0]; // Last added at level 1
        else if (wave == 6 && levelEnemies != null && levelEnemies.Length >= 2)
            introducedEnemy = levelEnemies[1]; // Last added at level 6
        else if (wave == 12 && levelEnemies != null && levelEnemies.Length >= 3)
            introducedEnemy = levelEnemies[2]; // Last added at level 12

        // Full row: enemies + correct pickup
        void AddFullRow(int rowIndex)
        {
            var row = new WaveRow();
            row.index = rowIndex;
            int enemyCount = Random.Range(1, 6);

            // If this level introduces a new enemy, add it first
            if (introducedEnemy != null)
            {
                row.boardObjects.Add(introducedEnemy);
                enemyCount--; // One slot used by introduced enemy
            }

            // Fill remaining slots
            for (int i = 0; i < enemyCount; i++)
            {
                var enemy = GetEnemyByRatio(enemies);
                row.boardObjects.Add(enemy);
            }

            // ballPickup on even waves (including wave 1), boxPickup on odd waves
            if (wave % 2 == 0 || wave == 1)
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


            // Fill remaining slots
            for (int i = 0; i < enemyCount; i++)
            {
                var enemy = GetEnemyByRatio(enemies);
                row.boardObjects.Add(enemy);
            }
            rows.Add(row);
        }

        if (wave == 1)
        {
            if (introducedEnemy != null)
            {
                // Row 6: introduced enemy + 2 random enemies + ballPickup
                var row = new WaveRow();
                row.index = 6;
                row.boardObjects.Add(introducedEnemy);
                for (int i = 0; i < 2; i++)
                {
                    var enemy = GetEnemyByRatio(enemies);
                    row.boardObjects.Add(enemy);
                }
                row.boardObjects.Add(ballPickup);
                rows.Add(row);
            }
            else
            {
                // Row 6: full row (enemies + ballPickup)
                AddFullRow(6);
            }
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

