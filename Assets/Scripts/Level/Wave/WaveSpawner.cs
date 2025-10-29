using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
  [SerializeField] private int spawnRow = 6;
  [SerializeField] private BoardState boardState;
  [SerializeField] private BoardManager boardManager;


  public IEnumerator SpawnWave(WaveContent waveContent)
  {
    int count = UnityEngine.Random.Range(1, 6);
    yield return StartCoroutine(SpawnWaveObjects(waveContent));
  }

  public IEnumerator SpawnWaveObjects(WaveContent waveContent)
  {
    var coroutines = new List<Coroutine>();
    var finished = 0;

    var spawnedEnemies = new List<Enemy>();
    foreach (var row in waveContent.waveRows)
    {
      foreach (var boardObject in row.boardObjects)
      {
        var cell = boardState.GetRandomEmptyCell(spawnRow);
        if (cell == null) continue;

        var (x, y) = cell.Value;
        var pos = boardState.GetWorldPosition(x, y);
        var obj = Instantiate(boardObject.prefab, transform);

        // todo refactor use interface
        if (obj.TryGetComponent<Enemy>(out var enemy))
        {
          boardState.PlaceObject(enemy, new Vector2Int(x, y));
          enemy.transform.position = pos;
          spawnedEnemies.Add(enemy);
        }
        else if (obj.TryGetComponent<PickupBall>(out var pickup))
        {
          boardState.PlaceObject(pickup, new Vector2Int(x, y));
          pickup.transform.position = pos;
        }
        else if (obj.TryGetComponent<PickupBox>(out var pickupBox))
        {
          boardState.PlaceObject(pickupBox, new Vector2Int(x, y));
          pickupBox.transform.position = pos;
        }
      }
    }


    foreach (var enemy in spawnedEnemies)
    {
      StartCoroutine(AnimateSpawnWrapper(enemy, () => finished++));
    }

    yield return new WaitUntil(() => finished >= spawnedEnemies.Count);
    yield return null;
  }

  private IEnumerator AnimateSpawnWrapper(Enemy enemy, Action onDone)
  {
    yield return enemy.AnimateSpawn(boardState);
    onDone?.Invoke();
  }
}
