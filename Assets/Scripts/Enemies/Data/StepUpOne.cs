using UnityEngine;

[CreateAssetMenu(menuName = "MiniPunBall/Enemy/MoveBehavior/StepUpOne")]
public class StepUpOne : MoveBehavior
{
  public override Vector2Int GetTargetCell(Enemy enemy, BoardState board)
      => enemy.CurrentCell - new Vector2Int(0, 1);
}