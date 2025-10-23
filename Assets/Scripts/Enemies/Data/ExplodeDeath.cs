using UnityEngine;

[CreateAssetMenu(menuName = "MiniPunBall/Enemy/DeathBehavior/Explode")]
public class ExplodeDeath : DeathBehavior
{
  public override void OnDeath(Enemy enemy, BoardState board)
  {
    Debug.Log($"{enemy.name} exploded!");
    // spawn particle, damage nearby enemies
  }
}