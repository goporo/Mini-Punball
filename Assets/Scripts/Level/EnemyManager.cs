using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
  private readonly List<Enemy> enemies = new();

  public int AliveCount => enemies.Count;
  public Enemy LowestHealth => enemies.OrderBy(e => e.CurrentHealth).FirstOrDefault();

  public void Register(Enemy e) => enemies.Add(e);
  public void Unregister(Enemy e) => enemies.Remove(e);

  [SerializeField] private GameObject enemyPrefab;
  private readonly int spawnRow = 6;
  [SerializeField] private BoardGrid board;


  public void SpawnWave(int waveNumber)
  {
    // Example: spawn 1–5 enemies at start
    int count = Random.Range(1, 6);
    SpawnEnemies(count);
  }

  public void SpawnEnemies(int count)
  {
    for (int i = 0; i < count; i++)
    {
      var cell = board.GetRandomEmptyCell(spawnRow);
      if (cell == null) return;

      var (x, y) = cell.Value;
      var pos = board.GetWorldPosition(x, y);
      var enemy = Instantiate(enemyPrefab, pos, Quaternion.identity);

      board.SetOccupant(x, y, enemy);
    }
  }

}