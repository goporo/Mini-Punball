using UnityEngine;

[CreateAssetMenu(menuName = "MiniPunBall/Enemy/DeathEffect/Explode")]
public class ExplodeDeath : DeathEffect
{
  public override void OnDeath(Enemy enemy, BoardState board)
  {
    Debug.Log($"{enemy.name} exploded!");
    // spawn particle, damage nearby enemies
  }
}