using System.Collections;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Reward : BoardObject, IPickupable, IAttacker, IDamageable
{
  [SerializeField] private EnemySkillBehavior EnemySkillBehavior;
  [SerializeField] private GameObject collectiblePrefab;

  private Collider rewardCollider;
  private bool isCollected = false;

  void Awake()
  {
    rewardCollider = GetComponent<Collider>();
  }

  public void OnPickup()
  {
    if (isCollected) return;
    isCollected = true;
    DeactivateCollider();
    var collectible = Instantiate(collectiblePrefab, transform.position, Quaternion.identity);
    HandleOnDeath();
  }

  public bool TakeDamage(DamageContext context)
  {
    OnPickup();
    return true;
  }

  private void DeactivateCollider()
  {
    rewardCollider.enabled = false;
  }

  public IEnumerator AnimateToPlayerAndCollect()
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
      yield return EnemySkillBehavior.AttackAndDie(this, new DamageContext { amount = 0 });
    }
  }
}