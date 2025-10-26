using System.Collections.Generic;
using UnityEngine;

public class BoardState : MonoBehaviour
{
  [SerializeField] private int width = 6;
  [SerializeField] private int height = 8;
  [SerializeField] private float cellSize = 1f;
  [SerializeField] private Vector3 origin = Vector3.zero;
  [SerializeField] private bool showBoardGrid = true;

  private BoardObject[,] grid;

  private void Awake()
  {
    grid = new BoardObject[width, height];
  }



  public bool IsEmpty(int x, int y)
  {
    return grid[x, y] == null;
  }


  public bool PlaceObject(BoardObject boardObject, Vector2Int cell)
  {
    if (!IsEmpty(cell.x, cell.y))
      return false;
    grid[cell.x, cell.y] = boardObject;
    boardObject.SetCell(cell);
    return true;
  }

  private int TotalObjects()
  {
    int count = 0;
    for (int x = 0; x < width; x++)
      for (int y = 0; y < height; y++)
        if (grid[x, y] != null) count++;
    return count;
  }

  public void ClearCell(Vector2Int cell)
  {
    grid[cell.x, cell.y] = null;
  }

  public (int x, int y)? GetRandomEmptyCell(int row)
  {
    var emptyCells = new List<(int, int)>();
    for (int x = 0; x < width; x++)
    {
      if (IsEmpty(x, row)) emptyCells.Add((x, row));
    }

    if (emptyCells.Count == 0) return null;
    return emptyCells[Random.Range(0, emptyCells.Count)];
  }

  public bool TryMove(BoardObject boardObject, Vector2Int target)
  {
    if (!IsInside(target) || IsOccupied(target)) return false;

    grid[boardObject.CurrentCell.x, boardObject.CurrentCell.y] = null;
    grid[target.x, target.y] = boardObject;
    boardObject.SetCell(target);
    return true;
  }

  public bool IsOccupied(Vector2Int pos) => grid[pos.x, pos.y] != null;

  public bool IsInside(Vector2Int pos) => pos.x >= 0 && pos.y >= 0 && pos.x < grid.GetLength(0) && pos.y < grid.GetLength(1);

  public Vector3 GetWorldPosition(int x, int y)
  {
    return origin + new Vector3(x * cellSize, 0, y * cellSize);
  }


  // Visualize grid in editor
  private void OnDrawGizmos()
  {
    if (showBoardGrid == false) return;
    Gizmos.color = Color.cyan;
    for (int x = 0; x < width; x++)
      for (int y = 0; y < height; y++)
      {
        Vector3 offset = new(0, 0.49f, 0);
        Vector3 pos = GetWorldPosition(x, y) - offset;
        Gizmos.DrawWireCube(pos, new Vector3(1f, 0f, 1f));
#if UNITY_EDITOR
        UnityEditor.Handles.Label(pos, $"({x},{y})");
#endif
      }
  }

}
