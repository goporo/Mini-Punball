using DG.Tweening;
using UnityEngine;

public class VFXMissile : VFXBase<TargetVFXParams>
{
  private readonly float duration = 0.8f;
  private Enemy target;
  private Tween moveTween;

  public override void OnSpawn(TargetVFXParams spawnParams)
  {
    base.OnSpawn();
    transform.position = spawnParams.Position;
    SetTarget(spawnParams.Target, spawnParams.Callback);
  }

  /// <summary>
  /// Sets the missile's target and starts the movement tween.
  /// </summary>
  public void SetTarget(Enemy enemy, System.Action onHit = null)
  {
    target = enemy;
    moveTween?.Kill();

    if (target == null)
    {
      RequestDone();
      return;
    }

    Vector3 start = transform.position;
    Vector3 end = target.Position;
    Vector3 mid;
    if (start == end)
    {
      mid = start + Vector3.up * Random.Range(2f, 3f);
    }
    else
    {
      mid = (start + end) / 2 + Vector3.up * Random.Range(1.5f, 2.5f)
            + new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
    }
    Vector3[] path = { start, mid, end };

    moveTween = transform.DOPath(path, duration, PathType.CatmullRom)
        .SetEase(Ease.Linear)
        .OnComplete(() =>
        {
          if (target != null)
            onHit?.Invoke();
          RequestDone();
        });
  }

  private void Update()
  {
    if (target == null)
    {
      moveTween?.Kill();
      RequestDone();
    }
  }

  public override void OnDespawn()
  {
    base.OnDespawn();
    moveTween?.Kill();
    moveTween = null;
    target = null;
  }
}