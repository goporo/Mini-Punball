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

    Tween scaleTween = AnimationUtility.PlayScale(boardObject.transform, Vector3.one, 0.25f, Ease.OutBack);
    if (scaleTween != null)
      yield return scaleTween.WaitForCompletion();
  }

  public void HandleOnDeath()
  {
    EventBus.Publish(new BoardObjectDeathEvent(this));
    Destroy(gameObject);
  }

}