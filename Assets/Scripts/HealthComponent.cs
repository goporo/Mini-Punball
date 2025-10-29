using System;
using DG.Tweening;
using UnityEngine;

public class HealthComponent : MonoBehaviour, IDamageable
{
  private int maxHealth;
  private int currentHealth;
  public event Action<HealthChangedEvent> OnHealthChanged;
  public event Action OnDied;
  private bool isBeingDestroyed = false;


  public void Init(int health)
  {
    maxHealth = health;
    currentHealth = maxHealth;
  }

  public void TakeDamage(DamageContext context)
  {
    currentHealth -= context.amount;
    if (!isBeingDestroyed) AnimationUtility.PlayBounce(transform);
    if (currentHealth <= 0)
    {
      currentHealth = 0;
      HandlePlayerDeath();
    }
    OnHealthChanged?.Invoke(new HealthChangedEvent(currentHealth, maxHealth));

  }

  private void HandlePlayerDeath()
  {
    OnDied?.Invoke();
    isBeingDestroyed = true;
  }


  private void OnDestroy()
  {
    if (transform != null)
      transform.DOKill();
  }

}


