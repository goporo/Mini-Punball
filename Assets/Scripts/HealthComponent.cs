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
  [Tooltip("The mesh that will have the bounce animation applied")]
  [SerializeField] private Transform targetMesh;
  private int maxHealth;
  private int currentHealth;
  public event Action<HealthChangedEvent> OnHealthChanged;
  public event Action OnDied;
  private bool isBeingDestroyed = false;
  private bool isDead = false;

  public int CurrentHealth => currentHealth;

  public void Init(int maxHealth)
  {
    this.maxHealth = maxHealth;
    currentHealth = maxHealth;
    isDead = false;
    isBeingDestroyed = false;
  }

  public bool TakeDamage(DamageContext context)
  {
    if (isDead) return true;
    currentHealth -= context.amount;
    if (!isBeingDestroyed) AnimationUtility.PlayBounce(targetMesh != null ? targetMesh : transform);
    if (currentHealth <= 0)
    {
      currentHealth = 0;
      HandleDeath();
    }
    OnHealthChanged?.Invoke(new HealthChangedEvent(currentHealth, maxHealth));
    return currentHealth <= 0;
  }

  private void HandleDeath()
  {
    if (isDead) return;
    isDead = true;
    OnDied?.Invoke();
    isBeingDestroyed = true;
  }

  private void OnDestroy()
  {
    (targetMesh != null ? targetMesh : transform).DOKill();
  }
}


