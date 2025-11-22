using DG.Tweening;
using UnityEngine;

public class VFXCometBase : VFXBase<TargetVFXParams>
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

    // Add z to the original position
    Vector3 start = transform.position;
    start.z += 6 - enemy.CurrentCell.y;
    transform.position = start;
    Vector3 end = target.Position;

    // Drop straight down from start to end
    Vector3[] path = { start, end };

    moveTween = transform.DOPath(path, duration, PathType.Linear)
        .SetEase(Ease.InQuad)
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