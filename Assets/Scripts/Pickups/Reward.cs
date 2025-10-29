using System.Collections;
using DG.Tweening;
using UnityEngine;

public class Reward : BoardObject, IPickupable, IAttacker
{
  [SerializeField] private SkillBehavior skillBehavior;
  public void OnPickup()
  {
    DeactivateCollider();
    StartCoroutine(AnimateToPlayerAndCollect());
  }

  private void DeactivateCollider()
  {
    var collider = GetComponent<Collider>();
    if (collider != null)
      collider.enabled = false;
  }

  private IEnumerator AnimateToPlayerAndCollect()
  {
    // Hardcoded player position for now (you can replace this with actual player position later)
    Vector3 playerPosition = new Vector3(3f, 0f, -5f);

    // Animate movement to player using AnimationUtility
    float animationDuration = 0.5f;
    Tween moveTween = AnimationUtility.PlayMove(transform, playerPosition, animationDuration, Ease.InQuad);
    if (moveTween != null)
      yield return moveTween.WaitForCompletion();

    // Publish the collected event to queue it in PickupManager
    EventBus.Publish(new PickupCollectedEvent(this));
  }

  public IEnumerator DoAttack(BoardState board)
  {
    if (CurrentCell.y == 0)
    {
      yield return skillBehavior.AttackAndDie(this, new DamageContext { amount = 0 });
    }
    else
    {
      yield return skillBehavior.UseSkill(this, board);
    }
  }
}