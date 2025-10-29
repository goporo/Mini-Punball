using UnityEngine;


[CreateAssetMenu(menuName = "MiniPunBall/Enemy/DeathEffect/Split")]
public class DeathSplit : DeathEffect
{
  public override void OnDeath(Enemy enemy, BoardState board)
  {
    Debug.Log($"{enemy.name} splits!");
    // spawn two new enemies with same moveBehavior
  }
}