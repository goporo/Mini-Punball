using System.Collections;
using DG.Tweening;
using UnityEngine;

public abstract class MoveBehavior : ScriptableObject
{
  public abstract Vector2Int GetTargetCell(BoardObject boardObject);

  public virtual IEnumerator AnimateMove(BoardObject boardObject, BoardState board)
  {
    Vector3 targetPos = board.GetWorldPosition(boardObject.CurrentCell.x, boardObject.CurrentCell.y);
    Tween moveTween = AnimationUtility.PlayMove(boardObject.transform, targetPos, 1f, Ease.OutCubic);
    if (moveTween != null)
      yield return moveTween.WaitForCompletion();
  }
}
