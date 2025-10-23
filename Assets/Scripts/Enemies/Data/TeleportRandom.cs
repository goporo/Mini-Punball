using System.Collections;
using UnityEngine;


[CreateAssetMenu(menuName = "MiniPunBall/Enemy/MoveBehavior/TeleportRandom")]
public class TeleportRandom : MoveBehavior
{
  public override Vector2Int GetTargetCell(Enemy enemy, BoardState board)
  {
    // var freeCells = board.GetAllFreeCells();
    // return freeCells[Random.Range(0, freeCells.Count)];
    return Vector2Int.zero;
  }
  public override IEnumerator AnimateMove(Enemy enemy, BoardState board)
  {
    // simple teleport VFX
    enemy.transform.position = board.GetWorldPosition(enemy.CurrentCell.x, enemy.CurrentCell.y);
    yield return new WaitForSeconds(0.1f);
  }
}