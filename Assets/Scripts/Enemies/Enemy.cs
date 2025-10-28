using System.Collections;
using UnityEngine;
using System;

public class Enemy : BoardObject
{
    [SerializeField] private EnemyUI enemyUI;

    [SerializeField] private EnemySO data;



    public IEnumerator AnimateSpawn(BoardState board)
    {
        yield return AnimateSpawn(this, board);
    }
    private int currentHealth;


    private void Awake()
    {
        currentHealth = data.baseHealth;
        enemyUI?.Init(data.baseHealth);
    }

    public void TakeDamage(DamageContext context)
    {
        currentHealth -= context.amount;
        if (currentHealth <= 0)
        {
            HandleOnDeath();
            return;
        }
        enemyUI?.OnTakeDamage(currentHealth, data.baseHealth);

    }

}
