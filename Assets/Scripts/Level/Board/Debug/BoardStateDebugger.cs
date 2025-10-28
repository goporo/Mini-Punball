using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class BoardStateDebugger : MonoBehaviour
{
  [Header("Debug Info")]
  [SerializeField, TextArea(10, 15)] private string gridVisualization = "";
  [SerializeField] private int objectCount = 0;
  [SerializeField] private int emptyCount = 0;

  private BoardState boardState;

  private void Awake()
  {
    boardState = GetComponent<BoardState>();
    if (boardState == null)
    {
      Debug.LogError("BoardStateDebugger needs BoardState component!");
      enabled = false;
    }
  }

  private void Start()
  {
    UpdateDisplay();

    // Subscribe to board changes for automatic updates
    BoardState.OnBoardChanged += UpdateDisplay;
  }

  private void OnDestroy()
  {
    // Unsubscribe to prevent memory leaks
    BoardState.OnBoardChanged -= UpdateDisplay;
  }

  private void OnValidate()
  {
    if (boardState != null)
      UpdateDisplay();
  }

  [ContextMenu("Update Display")]
  public void UpdateDisplay()
  {
    if (boardState == null) return;

    var objects = boardState.GetAllBoardObjects();
    objectCount = objects.Count;

    boardState.GetGridDimensions(out int width, out int height);
    emptyCount = (width * height) - objectCount;

    gridVisualization = CreateGridString(width, height);
  }

  private string CreateGridString(int width, int height)
  {
    const int colW = 3;                  // every column is 3 chars wide
    var nl = System.Environment.NewLine;
    var sb = new StringBuilder(width * height * (colW + 1) + 256);

    sb.Append($"Grid {width}x{height} | Objects: {objectCount} | Empty: {emptyCount}")
      .Append(nl).Append(nl);

    // Header
    sb.Append(' ', colW);
    for (int x = 0; x < width; x++)
      sb.Append(x.ToString("D2").PadLeft(colW));
    sb.Append(nl);

    // Rows (top -> bottom)
    for (int y = height - 1; y >= 0; y--)
    {
      sb.Append(y.ToString("D2")).Append(':');
      for (int x = 0; x < width; x++)
      {
        var c = boardState.GetObjectAt(x, y) is var obj && obj != null
            ? GetChar(obj)
            : '_';
        // each cell is exactly "[_]" (3 chars)
        sb.Append('[').Append(c).Append(']');
      }
      sb.Append(" :").Append(y.ToString("D2")).Append(nl);
    }

    // Footer
    sb.Append(' ', colW);
    for (int x = 0; x < width; x++)
      sb.Append(x.ToString("D2").PadLeft(colW));

    return sb.ToString();
  }
  private char GetChar(BoardObject obj)
  {
    if (obj == null) return '·';
    string name = obj.name.ToLower();

    if (name.Contains("enemy")) return 'E';
    if (name.Contains("pickup")) return 'P';
    if (name.Contains("ball")) return 'B';
    if (name.Contains("box")) return '□';

    return obj.name.Length > 0 ? obj.name[0] : 'X';
  }

  [ContextMenu("Log Grid")]
  public void LogGrid()
  {
    UpdateDisplay();
    Debug.Log($"Board Grid:\n{gridVisualization}");
  }
}