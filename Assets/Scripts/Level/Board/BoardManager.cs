using UnityEngine;

public class BoardManager : MonoBehaviour
{
  private GameObject currentBoard;

  // Called by LevelController to set up the board for a level
  public void InitBoard(LevelSO levelData)
  {
    if (currentBoard != null)
    {
      Destroy(currentBoard);
    }

    // Instantiate the board prefab
    currentBoard = Instantiate(levelData.boardPrefab, transform);

  }

  public GameObject GetCurrentBoard()
  {
    return currentBoard;
  }
}