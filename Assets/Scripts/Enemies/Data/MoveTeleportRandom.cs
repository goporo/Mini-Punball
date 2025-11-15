using System.Collections;
using DG.Tweening;
using UnityEngine;


[CreateAssetMenu(menuName = "MiniPunBall/Enemy/MoveTeleportRandom")]
public class MoveTeleportRandom : MoveBehavior
{
  public override Vector2Int GetTargetCell(BoardObject boardObject)
  {
    var freeCell = LevelContext.Instance.BoardState.GetRandomEmptyCell();
    if (freeCell != null)
    {
      return new Vector2Int(freeCell.Value.x, freeCell.Value.y);
    }
    return boardObject.CurrentCell;
  }
  public override IEnumerator AnimateMove(BoardObject boardObject, BoardState board)
  {
    // Warp VFX: shrink, move, grow
    // Shrink to disappear
    var shrinkTween = boardObject.transform.DOScale(Vector3.zero, 0.15f).SetEase(Ease.InBack);
    yield return shrinkTween.WaitForCompletion();

    // Move to new location (with offset if needed)
    boardObject.transform.position = boardObject.GetAlignedWorldPosition(board);

    // Grow to appear
    var growTween = boardObject.transform.DOScale(Vector3.one, 0.15f).SetEase(Ease.OutBack);
    yield return growTween.WaitForCompletion();
  }
}