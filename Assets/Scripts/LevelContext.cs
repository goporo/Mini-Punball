using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;


public class LevelContext : Singleton<LevelContext>
{
  public int TotalWaves { get; set; } = 0;
  public int CurrentWave { get; set; } = 0;
  private List<Enemy> enemies = new List<Enemy>();
  public bool CanShoot { get; set; } = true;


  public void RegisterEnemy(Enemy m) => enemies.Add(m);
  public void UnregisterEnemy(Enemy m) => enemies.Remove(m);

  public int TotalAlive => enemies.Count;
  public Enemy LowestHealth => enemies.OrderBy(m => m.currentHealth).FirstOrDefault();
  public Enemy HighestHealth => enemies.OrderByDescending(m => m.currentHealth).FirstOrDefault();

}
