using System.Collections;
using UnityEngine;


[CreateAssetMenu(menuName = "MiniPunBall/Enemy/MoveBehavior/MoveTeleportRandom")]
public class MoveTeleportRandom : MoveBehavior
{
  public override Vector2Int GetTargetCell(BoardObject boardObject, BoardState board)
  {
    // var freeCells = board.GetAllFreeCells();
    // return freeCells[Random.Range(0, freeCells.Count)];
    return Vector2Int.zero;
  }
  public override IEnumerator AnimateMove(BoardObject boardObject, BoardState board)
  {
    // simple teleport VFX
    boardObject.transform.position = board.GetWorldPosition(boardObject.CurrentCell.x, boardObject.CurrentCell.y);
    yield return new WaitForSeconds(0.1f);
  }
}