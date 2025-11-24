using UnityEngine;
using System.Collections;
using System.Linq;

public class VFXLightning : VFXBase<LightningVFXParams>
{
  [Header("Lightning Settings")]
  [SerializeField] private float lifetime = 0.18f;
  [SerializeField] private int segments = 10;
  [SerializeField] private float jaggedness = 0.35f;
  [SerializeField] private LineRenderer[] arcs; // assign in Inspector

  private float timer;

  protected override void Awake()
  {
    base.Awake();
    if (arcs == null || arcs.Length == 0)
      arcs = GetComponentsInChildren<LineRenderer>();
  }

  private LightningVFXParams currentParams;

  public override void OnSpawn(LightningVFXParams p)
  {
    base.OnSpawn();
    timer = lifetime;
    currentParams = p;
    StartCoroutine(JaggedAndFade());
  }

  private void GenerateArc(LineRenderer lr, Vector3 start, Vector3 end)
  {
    lr.positionCount = segments + 1;

    Vector3 dir = (end - start).normalized;

    for (int i = 0; i <= segments; i++)
    {
      float t = i / (float)segments;
      Vector3 pos = Vector3.Lerp(start, end, t);

      // More offset in the middle
      float strength = Mathf.Sin(t * Mathf.PI);
      float offset = Random.Range(-jaggedness, jaggedness) * strength;

      // Jaggedness only in XZ plane
      float xOffset = Random.Range(-offset, offset);
      float zOffset = Random.Range(-offset, offset);
      pos.x += xOffset;
      pos.z += zOffset;

      lr.SetPosition(i, pos);
    }
  }

  private IEnumerator JaggedAndFade()
  {
    float elapsed = 0f;
    while (elapsed < lifetime)
    {
      elapsed += Time.deltaTime;
      // Regenerate jagged arc every frame
      foreach (var arc in arcs)
        GenerateArc(arc, currentParams.StartPoint, currentParams.EndPoint);
      yield return null;
    }
    RequestDone();
  }
}
