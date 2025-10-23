using UnityEngine;

public abstract class BoardObject : MonoBehaviour
{
  public Vector2Int CurrentCell { get; set; }
  public abstract void SetCell(Vector2Int cell, BoardState board);
}