using UnityEngine;

public abstract class DeathBehavior : ScriptableObject
{
  public abstract void OnDeath(Enemy enemy, BoardState board);
}


