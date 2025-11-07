using UnityEngine;
using DG.Tweening;

public class Missile : RegisterableEffect
{
  private float speed = 5f;
  private Enemy target;
  private Tween moveTween;

  private void Awake()
  {
    RegisterEffect();
  }


  public void SetTarget(Enemy enemy, System.Action onHit = null)
  {
    target = enemy;
    if (target != null)
    {
      Vector3 start = transform.position;
      Vector3 end = target.Position;

      // Add random offset to the mid-point for path variation
      Vector3 mid = (start + end) / 2 + Vector3.up * Random.Range(1.5f, 2.5f);
      mid += new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));

      Vector3[] path = new Vector3[] { start, mid, end };
      float duration = Vector3.Distance(start, end) / speed;

      moveTween = transform.DOPath(path, duration, PathType.CatmullRom)
          .SetEase(Ease.Linear)
          .OnComplete(() =>
          {
            if (target != null)
            {
              Debug.Log("Missile hit " + target.name);
              onHit?.Invoke();
            }
            DestroyMissile();
          });
    }
  }

  private void Update()
  {
    if (target == null)
    {
      if (moveTween != null && moveTween.IsActive())
      {
        moveTween.Kill();
      }
      DestroyMissile();
    }
  }

  private void DestroyMissile()
  {
    UnregisterEffect();
    Destroy(gameObject);
  }
}