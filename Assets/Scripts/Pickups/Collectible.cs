using System.Collections;
using DG.Tweening;
using UnityEngine;

public class Collectible : MonoBehaviour, IPickupable
{
  private void Start()
  {
    // auto clockwise rotation animation Y axis use dotween
    IdleAnimation();
    SpawnAnimation();
  }

  private void SpawnAnimation()
  {
    // Save start position as ground
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
  public void OnPickup()
  {
    Debug.Log($"Picked up {gameObject.name}");
    StartCoroutine(AnimateToPlayerAndCollect());
  }

  private IEnumerator AnimateToPlayerAndCollect()
  {
    Vector3 playerPosition = new Vector3(3f, 0f, -5f);
    float animationDuration = 0.5f;
    Tween moveTween = AnimationUtility.PlayMove(transform, playerPosition, animationDuration, Ease.InQuad);
    yield return moveTween.WaitForCompletion();
    EventBus.Publish(new PickupCollectedEvent(this));
    Destroy(gameObject);

  }

  void IdleAnimation()
  {
    float duration = 3f;
    transform.DORotate(new Vector3(0, 360, 0), duration, RotateMode.FastBeyond360)
      .SetLoops(-1, LoopType.Restart)
      .SetEase(Ease.Linear);
  }
}