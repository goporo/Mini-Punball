using UnityEngine;

public class HealthComponent : MonoBehaviour, IDamageable
{
  [SerializeField] private int maxHealth = 100;
  public int CurrentHealth { get; private set; }

  private void Awake() => CurrentHealth = maxHealth;

  public void TakeDamage(DamageContext context)
  {
    if (CurrentHealth <= 0) return;

    CurrentHealth -= context.amount;
    Debug.Log($"{name} took {context.amount} damage ({CurrentHealth}/{maxHealth})");

    if (CurrentHealth <= 0)
    {
      CurrentHealth = 0;
      Die();
    }
  }

  protected virtual void Die()
  {
    Debug.Log($"{name} died.");
    Destroy(gameObject);
  }
}
