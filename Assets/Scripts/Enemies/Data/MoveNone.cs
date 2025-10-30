using System.Collections;
using UnityEngine;
using DG.Tweening;

[CreateAssetMenu(menuName = "MiniPunBall/MoveBehavior/NoMoveBehavior")]
public class MoveNone : MoveBehavior
{
  public override Vector2Int GetTargetCell(BoardObject boardObject)
  {
    return boardObject.CurrentCell;
  }

  public override IEnumerator AnimateMove(BoardObject boardObject, BoardState board)
  {
    yield break;

  }
}