using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
  [SerializeField] private BoardState boardState;
  [SerializeField] private BoardManager boardManager;


  public IEnumerator SpawnWave(WaveContent waveContent)
  {
    yield return StartCoroutine(SpawnWaveObjects(waveContent));
  }

  public IEnumerator SpawnWaveObjects(WaveContent waveContent)
  {
    var coroutines = new List<Coroutine>();
    var finished = 0;

    var spawnedEnemies = new List<Enemy>();
    if (waveContent.IsAfterBoss)
    {
      var availableEnemies = waveContent.AvailableEnemies.ToList();
      var pickedEnemies = availableEnemies.Count > 2
        ? availableEnemies.OrderBy(_ => UnityEngine.Random.value).Take(2).ToList()
        : availableEnemies;

      foreach (Enemy enemy in pickedEnemies)
      {
        var cell = boardState.GetRandomEmptyCell();
        if (cell == null) continue;

        var (x, y) = cell.Value;
        var obj = Instantiate(enemy.prefab, transform);

        if (obj.TryGetComponent<Enemy>(out var spawnedEnemy))
        {
          boardState.PlaceObject(spawnedEnemy, new Vector2Int(x, y));
          spawnedEnemy.Init(waveContent.HPMultiplier, waveContent.AttackMultiplier, boardState);
          spawnedEnemies.Add(spawnedEnemy);
        }
      }
    }
    else
    {
      foreach (var row in waveContent.waveRows)
      {
        foreach (var boardObject in row.boardObjects)
        {
          var size = boardObject.Size;
          var cell = boardState.GetRowEmptyCell(row.index, size);
          if (cell == null) continue;

          var (x, y) = cell.Value;

          var obj = Instantiate(boardObject.prefab, transform);

          // todo refactor use interface
          if (obj.TryGetComponent<Enemy>(out var enemy))
          {
            boardState.PlaceObject(enemy, new Vector2Int(x, y));
            enemy.Init(waveContent.HPMultiplier, waveContent.AttackMultiplier, boardState);
            spawnedEnemies.Add(enemy);
          }
          else if (obj.TryGetComponent<Reward>(out var reward))
          {
            boardState.PlaceObject(reward, new Vector2Int(x, y));
          }

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
