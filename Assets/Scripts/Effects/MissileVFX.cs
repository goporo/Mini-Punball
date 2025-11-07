using DG.Tweening;
using UnityEngine;

public class MissileVFX : BaseVFX<MissileVFXParams>
{
  private readonly float speed = 5f;
  private Enemy target;
  private Tween moveTween;

  public override void OnSpawn(MissileVFXParams spawnParams)
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
    Vector3 mid = (start + end) / 2 + Vector3.up * Random.Range(1.5f, 2.5f)
                  + new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
    Vector3[] path = { start, mid, end };
    float duration = Vector3.Distance(start, end) / speed;

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