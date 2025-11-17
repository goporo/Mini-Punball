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


  public bool TakeDamage(DamageContext context)
  {
    OnPickup();
    return true;
  }

  private void DeactivateCollider()
  {
    rewardCollider.enabled = false;
  }


  public void OnPickup()
  {
    if (isCollected) return;
    isCollected = true;
    DeactivateCollider();
    Instantiate(collectiblePrefab, transform.position, Quaternion.identity, transform.parent);
    HandleOnDeath();
  }


  public IEnumerator DoAttack(BoardState board)
  {
    if (CurrentCell.y == 0)
    {
      yield return EnemySkillBehavior.AttackAndDie(this, new PlayerDamageContext { });
    }
  }
}