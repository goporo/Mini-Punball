using UnityEngine;
using DG.Tweening;
using System;

public class Missile : MonoBehaviour
{
  public float speed = 5f;
  private Enemy target;
  private Tween moveTween;

  public void SetTarget(Enemy enemy, Action onHit = null)
  {
    target = enemy;
    if (target != null)
    {
      Vector3 start = transform.position;
      Vector3 end = target.Position;
      Vector3 mid = (start + end) / 2 + Vector3.up * 2f;

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
          Destroy(gameObject);
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
      Destroy(gameObject);
    }
  }
}