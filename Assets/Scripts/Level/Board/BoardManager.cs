using System;
using System.Collections;
using System.Linq;
using UnityEngine;

// Orchestrates the board state and manages board objects
[RequireComponent(typeof(BoardState))]
public class BoardManager : MonoBehaviour
{
  private BoardState board;
  private void Awake()
  {
    board = GetComponent<BoardState>();
  }

  void OnEnable()
  {
    EventBus.Subscribe<BoardObjectDeathEvent>(HandleBoardObjectDeath);
  }

  void OnDisable()
  {
    EventBus.Unsubscribe<BoardObjectDeathEvent>(HandleBoardObjectDeath);
  }

  private void HandleBoardObjectDeath(BoardObjectDeathEvent e)
  {
    board.ClearCell(e.BoardObject.CurrentCell);
  }

  public void InitBoard(LevelSO levelData)
  {
    Instantiate(levelData.boardPrefab, transform);

  }

  public IEnumerator StartAttack()
  {
    var finished = 0;
    var attackers = board.GetAllAttackers().ToList();
    foreach (var attacker in attackers)
    {
      StartCoroutine(StartAttackWrapper(attacker, () => finished++));
    }

    yield return new WaitUntil(() => finished >= attackers.Count);
  }

  public IEnumerator StartAttackWrapper(IAttacker attacker, Action onDone)
  {
    yield return attacker.DoAttack(board);
    onDone?.Invoke();
  }


  public IEnumerator StartMove()
  {
    var finished = 0;
    var boardObjects = board.GetAllBoardObjects().ToList();

    foreach (var boardObject in boardObjects)
    {
      StartCoroutine(MoveWrapper(boardObject, () => finished++));
    }

    yield return new WaitUntil(() => finished >= boardObjects.Count);
  }

  private IEnumerator MoveWrapper(BoardObject boardObject, Action onDone)
  {
    yield return boardObject.DoMove(board);
    onDone?.Invoke();
  }
}