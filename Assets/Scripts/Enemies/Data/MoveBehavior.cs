using System.Collections;
using DG.Tweening;
using UnityEngine;

public abstract class MoveBehavior : ScriptableObject
{
  public abstract Vector2Int GetTargetCell(BoardObject boardObject, BoardState board);

  public virtual IEnumerator AnimateMove(BoardObject boardObject, BoardState board)
  {
    Vector3 targetPos = board.GetWorldPosition(boardObject.CurrentCell.x, boardObject.CurrentCell.y);
    boardObject.transform.position = targetPos;
    yield break;
  }
}
