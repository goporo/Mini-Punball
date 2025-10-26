using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
  private GameObject currentBoard;

  private List<BoardObject> boardObjects = new();
  public List<BoardObject> BoardObjects => boardObjects;

  public void Register(BoardObject e) => boardObjects.Add(e);
  public void Unregister(BoardObject e) => boardObjects.Remove(e);
  private BoardState board;
  private void Awake()
  {
    board = GetComponent<BoardState>();
  }

  public void InitBoard(LevelSO levelData)
  {
    if (currentBoard != null)
    {
      Destroy(currentBoard);
    }

    // Instantiate the board prefab
    currentBoard = Instantiate(levelData.boardPrefab, transform);

  }




  public void HandleEnemyDeath(Enemy enemy)
  {
    if (enemy != null)
    {
      enemy.OnDeath -= HandleEnemyDeath;
    }
    Unregister(enemy);
    if (enemy != null)
    {
      Destroy(enemy.gameObject);
    }
  }

  public IEnumerator StartMove()
  {
    var moves = new List<Coroutine>();
    var finished = 0;

    foreach (var boardObject in boardObjects)
    {
      StartCoroutine(EnemyMoveWrapper(boardObject, () => finished++));
    }

    yield return new WaitUntil(() => finished >= boardObjects.Count);
    yield return null;
  }

  private IEnumerator EnemyMoveWrapper(BoardObject boardObject, Action onDone)
  {
    yield return boardObject.DoMove(board);
    onDone?.Invoke();
  }
}