using System.Collections;
using UnityEngine;

public abstract class MoveBehavior : ScriptableObject
{
  public abstract Vector2Int GetTargetCell(Enemy enemy, BoardState board);
  public virtual IEnumerator AnimateMove(Enemy enemy, BoardState board)
  {
    Debug.Log("Default AnimateMove: simple lerp " + enemy.name);
    Vector3 targetPos = board.GetWorldPosition(enemy.CurrentCell.x, enemy.CurrentCell.y);
    float elapsed = 0f;
    while (elapsed < 0.3f)
    {
      elapsed += Time.deltaTime;
      enemy.transform.position = Vector3.Lerp(enemy.transform.position, targetPos, elapsed / 0.3f);
      yield return null;
    }
  }
}
