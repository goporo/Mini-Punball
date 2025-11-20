using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public abstract class BoardObject : MonoBehaviour

{
  [Header("Visuals / Prefab")]
  public GameObject prefab;
  public Vector2Int CurrentCell { get; set; } // If size > 1, this is the bottom-left cell
  [Tooltip("Size in cells (width x height)")]
  [SerializeField] private Vector2Int size = Vector2Int.one; // width x height (e.g. 2x2 for boss)
  public Vector2Int Size => size;

  public MoveBehavior moveBehavior;
  public Vector3 Position => transform.position + Vector3.up * 1.0f;
  public bool CanAct { get; set; } = true;

  public Vector3 GetAlignedWorldPosition(BoardState board)
  {
    Vector3 basePos = board.GetWorldPosition(CurrentCell.x, CurrentCell.y);
    Vector3 offset = new Vector3((Size.x - 1) * board.CellSize / 2f, 0, (Size.y - 1) * board.CellSize / 2f);
    return basePos + offset;
  }

  public IEnumerable<Vector2Int> OccupiedCells
  {
    get
    {
      for (int x = 0; x < size.x; x++)
        for (int y = 0; y < size.y; y++)
          yield return CurrentCell + new Vector2Int(x, y);
    }
  }


  public void SetCell(Vector2Int cell)
  {
    CurrentCell = cell;
  }

  public IEnumerator DoMove(BoardState board)
  {
    if (!CanAct) yield break;
    var target = moveBehavior.GetTargetCell(this);

    if (board.TryMove(this, target))
    {
      yield return moveBehavior.AnimateMove(this, board);
    }

  }

  public virtual IEnumerator AnimateSpawn(BoardObject boardObject, BoardState board)
  {
    // Position already set by PlaceObject with offset applied
    boardObject.transform.position = boardObject.GetAlignedWorldPosition(board);
    boardObject.transform.localScale = Vector3.zero;

    Tween scaleTween = AnimationUtility.PlayScale(boardObject.transform, Vector3.one, 0.25f, Ease.OutBack);
    if (scaleTween != null)
      yield return scaleTween.WaitForCompletion();
  }

  public virtual void HandleOnDeath()
  {
    EventBus.Publish(new BoardObjectDeathEvent(this));
    Destroy(gameObject);
  }

  public void HandleOnSacrifice()
  {
    EventBus.Publish(new BoardObjectDeathEvent(this));
    Destroy(gameObject);
  }

}