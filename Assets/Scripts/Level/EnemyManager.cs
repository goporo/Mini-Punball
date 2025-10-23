using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
  private List<Enemy> enemies = new();
  public List<Enemy> Enemies => enemies;

  public int AliveCount => enemies.Count;
  public void Register(Enemy e) => enemies.Add(e);
  public void Unregister(Enemy e) => enemies.Remove(e);

  [SerializeField] private GameObject enemyPrefab;
  private readonly int spawnRow = 6;
  [SerializeField] private BoardState board;


  public void SpawnWave(int waveNumber)
  {
    int count = Random.Range(1, 6);
    SpawnEnemies(count);
  }

  public void SpawnEnemies(int count)
  {
    Debug.Log($"Spawning {count} enemies...");
    for (int i = 0; i < count; i++)
    {
      var cell = board.GetRandomEmptyCell(spawnRow);
      if (cell == null) return;

      var (x, y) = cell.Value;
      var pos = board.GetWorldPosition(x, y);
      var enemy = Instantiate(enemyPrefab, transform).GetComponent<Enemy>();
      board.PlaceObject(enemy, new Vector2Int(x, y));
      Register(enemy);
      enemy.OnDeath += HandleEnemyDeath;
    }
  }

  private void HandleEnemyDeath(Enemy enemy)
  {
    if (enemy != null)
    {
      enemy.OnDeath -= HandleEnemyDeath;
    }
    Unregister(enemy);
    if (enemy != null)
    {
      Destroy(enemy.gameObject);
    }
    // Optionally: other logic (score, effects, etc.)
  }

  public IEnumerator StartMove()
  {
    var moves = new List<Coroutine>();
    var finished = 0;

    foreach (var enemy in enemies)
    {
      StartCoroutine(EnemyMoveWrapper(enemy, () => finished++));
    }

    yield return new WaitUntil(() => finished >= enemies.Count);
    yield return null;
  }

  private IEnumerator EnemyMoveWrapper(Enemy enemy, System.Action onDone)
  {
    yield return enemy.DoMove(board);
    onDone?.Invoke();
  }

}