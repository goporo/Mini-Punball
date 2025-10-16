using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;


public class GameContext : Singleton<GameContext>
{
  private List<Monster> monsters = new List<Monster>();
  public bool CanShoot { get; set; } = true;



  // Register/unregister enemies
  public void RegisterMonster(Monster m) => monsters.Add(m);
  public void UnregisterMonster(Monster m) => monsters.Remove(m);

  // Aggregate queries
  public int TotalAlive => monsters.Count;
  public Monster LowestHealth => monsters.OrderBy(m => m.health).FirstOrDefault();
  public Monster HighestHealth => monsters.OrderByDescending(m => m.health).FirstOrDefault();
  public Vector3 AveragePosition => monsters.Count == 0 ? Vector3.zero : monsters.Select(m => m.transform.position).Aggregate(Vector3.zero, (a, b) => a + b) / monsters.Count;

  public bool HasAliveMonsters => monsters.Any();
}
