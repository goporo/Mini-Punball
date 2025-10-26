using System.Collections;
using UnityEngine;

public abstract class BoardObject : MonoBehaviour
{
  public GameObject prefab;
  public Vector2Int CurrentCell { get; set; }
  public int Size { get; }
  [SerializeField] protected EnemySO data;

  public void SetCell(Vector2Int cell)
  {
    CurrentCell = cell;
  }

  public IEnumerator DoMove(BoardState board)
  {
    var target = data.moveBehavior.GetTargetCell(this, board);
    Debug.Log($"Enemy {name} moving from {CurrentCell} to {target}");

    if (board.TryMove(this, target))
    {
      yield return data.moveBehavior.AnimateMove(this, board);
    }

    yield break;
  }


  public IEnumerator AnimateSpawn(BoardState board)
  {
    yield return data.moveBehavior.AnimateSpawn(this, board);
  }
}