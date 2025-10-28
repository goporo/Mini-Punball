using UnityEngine;

public abstract class DeathEffect : ScriptableObject
{
  public abstract void OnDeath(Enemy enemy, BoardState board);
}


