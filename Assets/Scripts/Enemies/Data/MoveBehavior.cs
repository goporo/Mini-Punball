using System.Collections;
using DG.Tweening;
using UnityEngine;

public abstract class MoveBehavior : ScriptableObject
{
  public abstract Vector2Int GetTargetCell(BoardObject boardObject, BoardState board);

  public virtual void TeleportTo(BoardObject boardObject, BoardState board)
  {
    Vector3 targetPos = board.GetWorldPosition(boardObject.CurrentCell.x, boardObject.CurrentCell.y);
    boardObject.transform.position = targetPos;
  }

  public virtual IEnumerator AnimateSpawn(BoardObject boardObject, BoardState board)
  {
    Vector3 targetPos = board.GetWorldPosition(boardObject.CurrentCell.x, boardObject.CurrentCell.y);
    boardObject.transform.position = targetPos;
    boardObject.transform.localScale = Vector3.zero;

    yield return boardObject.transform
        .DOScale(Vector3.one, 0.25f)
        .SetEase(Ease.OutBack)
        .WaitForCompletion();
  }

  public virtual IEnumerator AnimateMove(BoardObject boardObject, BoardState board)
  {
    Vector3 targetPos = board.GetWorldPosition(boardObject.CurrentCell.x, boardObject.CurrentCell.y);
    boardObject.transform.position = targetPos;
    yield break;
  }
}
