using UnityEngine;

[CreateAssetMenu(menuName = "MiniPunBall/Enemy/DeathEffect/None")]
public class DeathNone : DeathEffect
{
  public override void OnDeath(Enemy enemy, BoardState board) { }
}