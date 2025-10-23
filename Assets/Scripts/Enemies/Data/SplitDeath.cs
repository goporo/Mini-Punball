using UnityEngine;


[CreateAssetMenu(menuName = "MiniPunBall/Enemy/DeathBehavior/Split")]
public class SplitDeath : DeathBehavior
{
  public override void OnDeath(Enemy enemy, BoardState board)
  {
    Debug.Log($"{enemy.name} splits!");
    // spawn two new enemies with same moveBehavior
  }
}