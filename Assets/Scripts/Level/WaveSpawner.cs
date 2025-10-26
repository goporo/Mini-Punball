using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
  private readonly int spawnRow = 6;
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

        var enemy = obj.GetComponent<Enemy>();
        if (enemy != null)
        {
          boardState.PlaceObject(enemy, new Vector2Int(x, y));
          enemy.transform.position = pos;
          boardManager.Register(enemy);
          enemy.OnDeath += boardManager.HandleEnemyDeath;
          spawnedEnemies.Add(enemy);
        }
        else
        {
          var pickup = obj.GetComponent<PickupBall>();
          if (pickup != null)
          {
            boardState.PlaceObject(pickup, new Vector2Int(x, y));
            pickup.transform.position = pos;
            boardManager.Register(pickup);
          }
        }
      }
    }

    // for (int i = 0; i < count; i++)
    // {
    //   var cell = board.GetRandomEmptyCell(spawnRow);
    //   if (cell == null) yield break;

    //   var (x, y) = cell.Value;
    //   var pos = board.GetWorldPosition(x, y);
    //   var enemy = Instantiate(enemyPrefab, transform).GetComponent<Enemy>();
    //   board.PlaceObject(enemy, new Vector2Int(x, y));
    //   enemy.transform.position = pos;
    //   Register(enemy);
    //   enemy.OnDeath += HandleEnemyDeath;
    //   spawnedEnemies.Add(enemy);
    // }

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
