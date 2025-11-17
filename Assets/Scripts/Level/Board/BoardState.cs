using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BoardState : MonoBehaviour
{
  [SerializeField] private int width = 6;
  [SerializeField] private int height = 8;
  [SerializeField] private float cellSize = 1f;
  public float CellSize => cellSize;
  [SerializeField] private Vector3 origin = Vector3.zero;
  [SerializeField] private bool showBoardGrid = true;

  private BoardObject[,] grid;

  public static System.Action OnBoardChanged;

  private void Awake()
  {
    grid = new BoardObject[width, height];
  }

  public List<BoardObject> GetAllBoardObjects()
  {
    var objects = new HashSet<BoardObject>();
    for (int x = 0; x < width; x++)
      for (int y = 0; y < height; y++)
        if (grid[x, y] != null)
          objects.Add(grid[x, y]);
    return objects.ToList();
  }

  public List<Enemy> GetAllEnemies()
  {
    return GetAllBoardObjects().OfType<Enemy>().ToList();
  }

  public Enemy GetRandomEnemy(Enemy exclude = null)
  {
    var enemies = exclude == null
        ? GetAllEnemies()
        : GetAllEnemies().Where(e => e != exclude).ToList();
    if (enemies.Count == 0) return null;
    return enemies[Random.Range(0, enemies.Count)];
  }

  public List<Enemy> GetLowestHealthEnemies(int count, Enemy exclude = null)
  {
    var enemies = exclude == null
      ? GetAllEnemies()
      : GetAllEnemies().Where(e => e != exclude).ToList();

    var sorted = enemies
      .OrderBy(e => e.HealthComponent.CurrentHealth)
      .ToList();

    if (sorted.Count == 0)
    {
      if (exclude != null)
        return Enumerable.Repeat(exclude, count).ToList();
      return new List<Enemy>();
    }

    // Fill up with the first enemy if not enough
    while (sorted.Count < count)
    {
      sorted.Add(sorted[0]);
    }

    return sorted.Take(count).ToList();
  }

  public Enemy[] GetTwoLowestHealthEnemies(Enemy exclude = null)
  {
    var enemies = exclude == null
        ? GetAllEnemies()
        : GetAllEnemies().Where(e => e != exclude).ToList();

    if (enemies.Count == 0)
      return null;
    if (enemies.Count == 1)
      return new Enemy[] { enemies[0], enemies[0] };

    return enemies.OrderBy(e => e.HealthComponent.CurrentHealth).Take(2).ToArray();
  }

  public List<IAttacker> GetAllAttackers()
  {
    return GetAllBoardObjects().OfType<IAttacker>().ToList();
  }

  public bool IsEmpty(int x, int y)
  {
    if (!IsInside(new Vector2Int(x, y))) return false;
    return grid[x, y] == null;
  }


  public List<BoardObject> GetSurroundingObjects(Vector2Int center, int radius)
  {
    var objects = new HashSet<BoardObject>();
    for (int dx = -radius; dx <= radius; dx++)
    {
      for (int dy = -radius; dy <= radius; dy++)
      {
        if (dx == 0 && dy == 0) continue; // Skip center
        Vector2Int pos = new(center.x + dx, center.y + dy);
        if (IsInside(pos) && grid[pos.x, pos.y] != null)
        {
          objects.Add(grid[pos.x, pos.y]);
        }
      }
    }
    return objects.ToList();
  }

  public List<BoardObject> GetRowObjects(Vector2Int original)
  {
    var rowObjects = new HashSet<BoardObject>();
    for (int x = 0; x < width; x++)
    {
      if (grid[x, original.y] != null)
      {
        rowObjects.Add(grid[x, original.y]);
      }
    }
    return rowObjects.ToList();
  }

  public List<BoardObject> GetColumnObjects(Vector2Int original)
  {
    var columnObjects = new HashSet<BoardObject>();
    for (int y = 0; y < height; y++)
    {
      if (grid[original.x, y] != null)
      {
        columnObjects.Add(grid[original.x, y]);
      }
    }
    return columnObjects.ToList();
  }


  public bool PlaceObject(BoardObject boardObject, Vector2Int cell)
  {
    boardObject.SetCell(cell);
    // Check if all occupied cells are empty
    foreach (var occupiedCell in boardObject.OccupiedCells)
    {
      if (!IsInside(occupiedCell) || (grid[occupiedCell.x, occupiedCell.y] != null))
        return false;
    }
    // Place object in all occupied cells
    foreach (var occupiedCell in boardObject.OccupiedCells)
    {
      grid[occupiedCell.x, occupiedCell.y] = boardObject;
    }
    boardObject.transform.position = boardObject.GetAlignedWorldPosition(this);

    OnBoardChanged?.Invoke();

    return true;
  }

  public void ClearCell(Vector2Int cell)
  {
    var obj = grid[cell.x, cell.y];
    if (obj == null) return;

    // Clear all cells occupied by this object
    foreach (var occupiedCell in obj.OccupiedCells)
    {
      if (IsInside(occupiedCell))
        grid[occupiedCell.x, occupiedCell.y] = null;
    }

    // Notify debugger
    OnBoardChanged?.Invoke();
  }

  public void ClearCells(IEnumerable<Vector2Int> cells)
  {
    foreach (var cell in cells)
    {
      ClearCell(cell);
    }
  }

  public (int x, int y)? GetRowEmptyCell(int row, Vector2Int requireSize)
  {
    var emptyCells = new List<(int, int)>();
    for (int x = 0; x <= width - requireSize.x; x++)
    {
      bool allEmpty = true;
      for (int dx = 0; dx < requireSize.x; dx++)
      {
        for (int dy = 0; dy < requireSize.y; dy++)
        {
          int checkX = x + dx;
          int checkY = row + dy;
          if (!IsInside(new Vector2Int(checkX, checkY)) || !IsEmpty(checkX, checkY))
          {
            allEmpty = false;
            break;
          }
        }
        if (!allEmpty) break;
      }
      if (allEmpty)
        emptyCells.Add((x, row));
    }

    if (emptyCells.Count == 0) return null;
    return emptyCells[Random.Range(0, emptyCells.Count)];
  }

  public (int x, int y)? GetRandomEmptyCell()
  {
    int[] validRows = { 1, 2, 3, 4, 5, 6 };
    var emptyCells = new List<(int, int)>();
    foreach (int row in validRows)
    {
      for (int x = 0; x < width; x++)
      {
        if (IsEmpty(x, row)) emptyCells.Add((x, row));
      }
    }

    if (emptyCells.Count == 0) return null;
    return emptyCells[Random.Range(0, emptyCells.Count)];
  }

  public (int x, int y)? GetRandom2x2EmptyCell(int[] validRows = null)
  {
    if (validRows == null || !validRows.Any())
    {
      validRows = new int[] { 1, 2, 3, 4, 5 };
    }
    var emptyCells = new List<(int, int)>();
    foreach (int row in validRows)
    {
      for (int x = 0; x < width - 1; x++) // -1 to ensure 2x2 fits
      {
        // Check all 4 cells in the 2x2 block
        if (IsEmpty(x, row) && IsEmpty(x + 1, row) && IsEmpty(x, row + 1) && IsEmpty(x + 1, row + 1))
        {
          emptyCells.Add((x, row)); // bottom-left origin
        }
      }
    }
    if (emptyCells.Count == 0) return null;
    return emptyCells[Random.Range(0, emptyCells.Count)];
  }

  public bool TryMove(BoardObject boardObject, Vector2Int target)
  {
    // Calculate all cells the object would occupy at the target position
    var targetCells = new List<Vector2Int>();
    for (int x = 0; x < boardObject.Size.x; x++)
    {
      for (int y = 0; y < boardObject.Size.y; y++)
      {
        var cell = target + new Vector2Int(x, y);
        if (!IsInside(cell))
          return false;
        targetCells.Add(cell);
      }
    }

    // Check if all target cells are empty (or occupied by the same object)
    foreach (var cell in targetCells)
    {
      if (grid[cell.x, cell.y] != null && grid[cell.x, cell.y] != boardObject)
        return false;
    }

    // Clear old cells
    foreach (var cell in boardObject.OccupiedCells)
    {
      grid[cell.x, cell.y] = null;
    }

    // Set new position
    boardObject.SetCell(target);

    // Occupy new cells
    foreach (var cell in boardObject.OccupiedCells)
    {
      grid[cell.x, cell.y] = boardObject;
    }

    OnBoardChanged?.Invoke();

    return true;
  }

  public void ForceClearBoard()
  {
    GetAllBoardObjects().ToList().ForEach(obj =>
    {
      obj.HandleOnSacrifice();
      ClearCells(obj.OccupiedCells);
    });

    OnBoardChanged?.Invoke();
  }


  public bool IsInside(Vector2Int pos) => pos.x >= 0 && pos.y >= 0 && pos.x < grid.GetLength(0) && pos.y < grid.GetLength(1);

  public Vector3 GetWorldPosition(int x, int y)
  {
    return origin + new Vector3(x * cellSize, 0, y * cellSize);
  }

  public BoardObject GetObjectAt(int x, int y)
  {
    if (!IsInside(new Vector2Int(x, y))) return null;
    return grid[x, y];
  }

  public void GetGridDimensions(out int gridWidth, out int gridHeight)
  {
    gridWidth = width;
    gridHeight = height;
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
