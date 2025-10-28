using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public abstract class BoardObject : MonoBehaviour
{
  [Header("Visuals / Prefab")]
  public GameObject prefab;
  public Vector2Int CurrentCell { get; set; }
  public int Size { get; }
  public MoveBehavior moveBehavior;


  public void SetCell(Vector2Int cell)
  {
    CurrentCell = cell;
  }

  public IEnumerator DoMove(BoardState board)
  {
    var target = moveBehavior.GetTargetCell(this, board);
    // Debug.Log($"Enemy {name} moving from {CurrentCell} to {target}");

    if (board.TryMove(this, target))
    {
      yield return moveBehavior.AnimateMove(this, board);
    }

    yield break;
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

  public void HandleOnDeath()
  {
    EventBus.Publish(new BoardObjectDeathEvent(this));
    Destroy(gameObject);
  }

}