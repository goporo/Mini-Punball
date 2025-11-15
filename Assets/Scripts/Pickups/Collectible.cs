using System.Collections;
using DG.Tweening;
using UnityEngine;

public class Collectible : MonoBehaviour
{
  public CollectibleType Type;
  private float animationDuration = 0.25f;

  private void Start()
  {
    IdleAnimation();
    SpawnAnimation();
    EventBus.Publish(new OnCollectibleSpawnEvent(this));

  }

  private void SpawnAnimation()
  {
    Vector3 startPos = transform.position;
    float upHeight = 1.5f;
    float bounceHeight = 0.5f;
    float durationUp = 0.35f;
    float durationDown = 0.25f;
    float durationBounce = 0.2f;

    // Randomize X/Z offset
    float xOffset = Random.Range(-0.5f, 0.5f);
    float zOffset = Random.Range(-0.5f, 0.5f);
    Vector3 peakPos = startPos + new Vector3(xOffset, upHeight, zOffset);

    // Sequence: fire up, fall to ground, bounce up, fall to ground
    Sequence seq = DOTween.Sequence();
    seq.Append(transform.DOMove(peakPos, durationUp).SetEase(Ease.OutQuad));
    seq.Append(transform.DOMove(startPos, durationDown).SetEase(Ease.InQuad));
    seq.Append(transform.DOMove(startPos + Vector3.up * bounceHeight, durationBounce).SetEase(Ease.OutQuad));
    seq.Append(transform.DOMove(startPos, durationBounce).SetEase(Ease.InQuad));
    seq.Play();
  }

  private void IdleAnimation()
  {
    float duration = 3f;
    transform.DORotate(new Vector3(0, 360, 0), duration, RotateMode.FastBeyond360)
      .SetLoops(-1, LoopType.Restart)
      .SetEase(Ease.Linear);
  }

  public IEnumerator AnimateToPlayer()
  {
    DOTween.Kill(transform);
    Vector3 playerPosition = LevelContext.Instance.Player.transform.position;
    Tween moveTween = AnimationUtility.PlayMove(transform, playerPosition, animationDuration, Ease.InQuad);
    yield return moveTween.WaitForCompletion();
    yield return null;
    Destroy(gameObject);
  }

}

public enum CollectibleType
{
  Ball,
  Box,
  Health,
  Coin
}