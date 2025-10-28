using UnityEngine;

[CreateAssetMenu(menuName = "MiniPunBall/Enemy/DeathEffect/None")]
public class SimpleDeath : DeathEffect
{
  public override void OnDeath(Enemy enemy, BoardState board) { }
}