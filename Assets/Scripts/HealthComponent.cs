using System;
using DG.Tweening;
using UnityEngine;


public struct HealthChangedEvent
{
  public int Current;
  public int Max;

  public HealthChangedEvent(int current, int max)
  {
    Current = current;
    Max = max;
  }
}

public class HealthComponent : MonoBehaviour, IDamageable
{
  private int maxHealth;
  private int currentHealth;
  public event Action<HealthChangedEvent> OnHealthChanged;
  public event Action OnDied;
  private bool isBeingDestroyed = false;


  public void Init(int maxHealth)
  {
    this.maxHealth = maxHealth;
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


