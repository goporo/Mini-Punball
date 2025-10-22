using UnityEngine;

public class BoardGrid : MonoBehaviour
{
  [SerializeField] private int width = 6;
  [SerializeField] private int height = 8;
  [SerializeField] private float cellSize = 1f;
  [SerializeField] private Vector3 origin = Vector3.zero;
  [SerializeField] private bool showBoardGrid = true;

  private GameObject[,] grid;

  private void Awake()
  {
    grid = new GameObject[width, height];
  }

  public Vector3 GetWorldPosition(int x, int y)
  {
    return origin + new Vector3(x * cellSize, 0, y * cellSize);
  }

  public bool IsEmpty(int x, int y)
  {
    return grid[x, y] == null;
  }

  public void SetOccupant(int x, int y, GameObject obj)
  {
    grid[x, y] = obj;
  }

  public void Clear(int x, int y)
  {
    grid[x, y] = null;
  }

  public (int x, int y)? GetRandomEmptyCell(int row)
  {
    var emptyCells = new System.Collections.Generic.List<(int, int)>();
    for (int x = 0; x < width; x++)
    {
      if (IsEmpty(x, row)) emptyCells.Add((x, row));
    }

    if (emptyCells.Count == 0) return null;
    return emptyCells[Random.Range(0, emptyCells.Count)];
  }

  // Visualize grid in editor
  private void OnDrawGizmos()
  {
    if (showBoardGrid == false) return;
    Gizmos.color = Color.cyan;
    for (int x = 0; x < width; x++)
      for (int y = 0; y < height; y++)
      {
        Vector3 pos = GetWorldPosition(x, y);
        Gizmos.DrawWireCube(pos, Vector3.one * (cellSize * 0.9f));
      }
  }
}
