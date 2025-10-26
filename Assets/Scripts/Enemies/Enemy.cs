using System.Collections;
using UnityEngine;

public class Enemy : BoardObject
{
    [SerializeField] private EnemyUI enemyUI;


    private int currentHealth;
    private DeathBehavior DeathBehavior => data.deathBehavior;


    public event System.Action<Enemy> OnDeath;

    private void Awake()
    {
        currentHealth = data.baseHealth;
        enemyUI?.Init(data.baseHealth);
    }




    public void TakeDamage(DamageContext context)
    {
        currentHealth -= context.amount;
        enemyUI?.OnTakeDamage(currentHealth, data.baseHealth);

        if (currentHealth <= 0)
        {
            data.deathBehavior?.OnDeath(this, board: null);
            OnDeath?.Invoke(this);
        }
    }

}
