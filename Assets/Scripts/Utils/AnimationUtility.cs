using UnityEngine;
using DG.Tweening;

public static class AnimationUtility
{
  public static void PlayBounce(Transform target, float bounceScale = 1.2f, float upDuration = 0.1f, float downDuration = 0.15f)
  {
    if (target == null || !target.gameObject.activeInHierarchy) return;

    target.DOKill();
    target.localScale = Vector3.one;
    target.DOScale(bounceScale, upDuration)
        .SetEase(Ease.OutQuad)
        .OnComplete(() =>
        {
          if (target != null && target.gameObject.activeInHierarchy)
            target.DOScale(1f, downDuration).SetEase(Ease.InQuad);
        });
  }

  public static Tween PlayMove(Transform target, Vector3 targetPos, float duration = 1f, Ease ease = Ease.OutCubic)
  {
    if (target == null || !target.gameObject.activeInHierarchy) return null;
    return target.DOMove(targetPos, duration).SetEase(ease);
  }

  public static Tween PlayScale(Transform target, Vector3 targetScale, float duration = 0.25f, Ease ease = Ease.OutBack)
  {
    if (target == null || !target.gameObject.activeInHierarchy) return null;
    return target.DOScale(targetScale, duration).SetEase(ease);
  }
}