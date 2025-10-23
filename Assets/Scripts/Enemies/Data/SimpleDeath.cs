using UnityEngine;

[CreateAssetMenu(menuName = "MiniPunBall/Enemy/DeathBehavior/None")]
public class SimpleDeath : DeathBehavior
{
  public override void OnDeath(Enemy enemy, BoardState board) { }
}