using UnityEngine;

public class HealthComponent : MonoBehaviour, IDamageable
{
  private int maxHealth;
  private int currentHealth;

  public void Init(int health)
  {
    maxHealth = health;
    currentHealth = maxHealth;
  }

  public void TakeDamage(DamageContext context)
  {

  }

}
