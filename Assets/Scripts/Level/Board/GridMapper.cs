using UnityEngine;

public class GridMapper : MonoBehaviour
{
  private Transform origin;
  private float cellSize = 1f;

  public static Vector3 GetWorldPosition(int x, int y)
    => GridMapper.instance.origin.position + new Vector3(x * GridMapper.instance.cellSize, 0, y * GridMapper.instance.cellSize);

  private static GridMapper instance;

  private void Awake()
  {
    instance = this;
  }
}