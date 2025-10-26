using System.Collections;
using UnityEngine;
using DG.Tweening;

[CreateAssetMenu(menuName = "MiniPunBall/MoveBehavior/StepUpOne")]
public class StepUpOne : MoveBehavior
{
  public override Vector2Int GetTargetCell(BoardObject boardObject, BoardState board)
      => boardObject.CurrentCell - new Vector2Int(0, 1);

  public override IEnumerator AnimateMove(BoardObject boardObject, BoardState board)
  {
    Vector3 targetPos = board.GetWorldPosition(boardObject.CurrentCell.x, boardObject.CurrentCell.y);
    yield return boardObject.transform
      .DOMove(targetPos, 1f)
      .SetEase(Ease.OutCubic)
      .WaitForCompletion();
  }
}