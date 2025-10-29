using System.Collections;
using UnityEngine;
using System;
using DG.Tweening;

public class Enemy : BoardObject
{
    [SerializeField] private EnemyUI enemyUI;

    [SerializeField] private EnemySO data;
    [SerializeField] private HealthComponent healthComponent;




    public IEnumerator AnimateSpawn(BoardState board)
    {
        yield return AnimateSpawn(this, board);
    }
    private int currentHealth;


    private void Awake()
    {
        currentHealth = data.baseHealth;
        enemyUI?.Init(data.baseHealth);
        healthComponent = GetComponent<HealthComponent>();
    }

    private bool isBeingDestroyed = false;

    private void OnDestroy()
    {
        isBeingDestroyed = true;
        if (transform != null)
            transform.DOKill();
    }

    public void TakeDamage(DamageContext context)
    {
        currentHealth -= context.amount;

        // Use the utility for animation
        if (!isBeingDestroyed)
            AnimationUtility.PlayBounce(transform);

        if (currentHealth <= 0)
        {
            HandleOnDeath();
            return;
        }
        enemyUI?.OnTakeDamage(currentHealth, data.baseHealth);
    }

}
