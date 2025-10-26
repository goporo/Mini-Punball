using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RandomWaveListSO", menuName = "MiniPunBall/RandomWaveListSO", order = 0)]
public class RandomWaveListSO : WaveListSO
{
    [SerializeField] private BoardObject[] availableBoardObjects;
    // [SerializeField] private GameObject bossPrefab;
    [SerializeField] private BoardObject ballPickup;
    [SerializeField] private BoardObject boxPickup;


    [SerializeField] private List<int> bossWaveIndices = new() { 20 }; // e.g., {20, 40} for 40-wave level

    public override WaveContent GenerateWave(int waveNumber)
    {
        var content = new WaveContent();

        // Boss wave rule: check if this wave is a boss wave
        // if (bossWaveIndices.Contains(waveNumber))
        // {
        //     content.waveRows = new WaveRow[1];
        //     var bossRow = new WaveRow();
        //     bossRow.boardObjects.Add(new BoardObject { prefab = bossPrefab });
        //     content.waveRows[0] = bossRow;
        //     return content;
        // }

        // Before boss wave: empty wave
        foreach (var bossIndex in bossWaveIndices)
        {
            if (waveNumber == bossIndex - 1)
            {
                content.waveRows = new WaveRow[0];
                return content;
            }
        }

        // First wave: treat as even
        bool isEven = waveNumber % 2 == 0 || waveNumber == 1;

        var row = new WaveRow();
        int enemyCount = Random.Range(1, 6);
        for (int i = 0; i < enemyCount; i++)
        {
            var enemy = availableBoardObjects[Random.Range(0, availableBoardObjects.Length)];
            row.boardObjects.Add(enemy);
        }

        if (isEven)
        {
            row.boardObjects.Add(ballPickup);
        }
        else
        {
            row.boardObjects.Add(boxPickup);
        }

        content.waveRows = new WaveRow[] { row };
        return content;
    }
}

[System.Serializable]
public class WaveContent
{
    public WaveRow[] waveRows;
}

[System.Serializable]
public class WaveRow
{
    public int rowIndex;
    public List<BoardObject> boardObjects = new();
}

