using UnityEngine;
using System;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] protected int maxHealth = 10;
    [SerializeField] protected EnemyUI enemyUI;

    public int currentHealth;
    public bool IsAlive => currentHealth > 0;
    public Vector3 Position => transform.position;

    public static event Action<Enemy> OnEnemyDied;

    protected virtual void Awake()
    {
        currentHealth = maxHealth;
        enemyUI?.Init(maxHealth);
    }

    public virtual void TakeDamage(DamageContext context)
    {
        int amount = context.amount;
        currentHealth -= amount;
        enemyUI?.OnTakeDamage(currentHealth, maxHealth);
        Debug.Log($"{name} took {amount} damage, hp = {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
        if (context.statusEffect != null)
        {
            ApplyStatusEffect(context.statusEffect);
        }
    }

    public virtual void ApplyStatusEffect(IStatusEffect statusEffect)
    {
        statusEffect.Apply(this);
    }

    protected virtual void Die()
    {
        OnEnemyDied?.Invoke(this);
        Destroy(gameObject);
    }

    // Optional: override for unique enemy logic
    public abstract void PerformBehavior();
}
