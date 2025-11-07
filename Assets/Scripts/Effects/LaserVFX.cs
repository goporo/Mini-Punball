using UnityEngine;

/// <summary>
/// Pooled laser VFX effect
/// </summary>
[RequireComponent(typeof(LineRenderer))]
public class LaserVFX : BaseVFX<LaserVFXParams>
{
  [Header("Laser Settings")]
  [SerializeField] private float laserLifetime = 0.25f;

  private LineRenderer lineRenderer;

  protected override void Awake()
  {
    base.Awake();
    lineRenderer = GetComponent<LineRenderer>();
  }

  public override void OnSpawn(LaserVFXParams spawnParams)
  {
    base.OnSpawn();

    if (lineRenderer != null)
    {
      lineRenderer.SetPosition(0, spawnParams.StartPoint);
      lineRenderer.SetPosition(1, spawnParams.EndPoint);
    }
    StartCoroutine(WaitFinish());

  }


  private System.Collections.IEnumerator WaitFinish()
  {
    yield return new WaitForSeconds(laserLifetime);
    RequestDone();
  }


}
