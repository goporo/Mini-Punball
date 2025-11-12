using UnityEngine;

public class Projectile : RegisterableEffect
{
  private PlayerRunStats target;
  private System.Action onHit;
  private float speed = 20f;

  private void Awake()
  {
    RegisterEffect();
  }

  public void SetTarget(PlayerRunStats target, System.Action onHit)
  {
    this.target = target;
    this.onHit = onHit;
  }

  private void Update()
  {
    if (target == null)
    {
      UnregisterEffect();
      Destroy(gameObject);
      return;
    }

    float step = speed * Time.deltaTime;
    transform.position = Vector3.MoveTowards(transform.position, target.transform.position, step);

    if (Vector3.Distance(transform.position, target.transform.position) < 0.1f)
    {
      onHit?.Invoke();
      UnregisterEffect();
      Destroy(gameObject);
    }
  }
}