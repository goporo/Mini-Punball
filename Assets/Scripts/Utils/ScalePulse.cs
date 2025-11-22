using UnityEngine;

public class ScalePulse : MonoBehaviour
{
  public float pulseSpeed = 4f;
  public float pulseAmount = 0.2f;

  private Vector3 baseScale;

  void Awake()
  {
    baseScale = transform.localScale;
  }

  void Update()
  {
    float scale = 1f + Mathf.Sin(Time.time * pulseSpeed) * pulseAmount;
    transform.localScale = baseScale * scale;
  }
}
