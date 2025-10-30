using System.Collections;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Reward : BoardObject, IPickupable, IAttacker
{
  [SerializeField] private SkillBehavior skillBehavior;
  private Collider rewardCollider;

  void Awake()
  {
    rewardCollider = GetComponent<Collider>();
  }
  public void OnPickup()
  {
    DeactivateCollider();
    StartCoroutine(AnimateToPlayerAndCollect());
  }

  private void DeactivateCollider()
  {
    rewardCollider.enabled = false;
  }

  private IEnumerator AnimateToPlayerAndCollect()
  {
    Vector3 playerPosition = new Vector3(3f, 0f, -5f);
    float animationDuration = 0.5f;
    Tween moveTween = AnimationUtility.PlayMove(transform, playerPosition, animationDuration, Ease.InQuad);
    yield return moveTween.WaitForCompletion();
    EventBus.Publish(new PickupCollectedEvent(this));
    HandleOnDeath();
  }

  public IEnumerator DoAttack(BoardState board)
  {
    if (CurrentCell.y == 0)
    {
      yield return skillBehavior.AttackAndDie(this, new DamageContext { amount = 0 });
    }
  }
}