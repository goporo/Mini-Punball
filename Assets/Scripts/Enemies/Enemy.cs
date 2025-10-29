using System.Collections;
using UnityEngine;
using System;
using DG.Tweening;

[RequireComponent(typeof(HealthComponent))]
public class Enemy : BoardObject, IAttacker
{
    [SerializeField] private EnemyUI enemyUI;

    [SerializeField] private EnemySO data;
    private HealthComponent healthComponent;
    private SkillBehavior skillBehavior => data.skillBehavior;

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
        healthComponent.Init(currentHealth);
    }

    void OnEnable()
    {
        healthComponent.OnDied += HandleOnDeath;
    }

    void OnDisable()
    {
        healthComponent.OnDied -= HandleOnDeath;
    }

    public IEnumerator DoAttack(BoardState board)
    {
        if (CurrentCell.y == 0)
        {
            yield return skillBehavior.AttackAndDie(this, new DamageContext { amount = data.baseAttack });
        }
        else
        {
            yield return skillBehavior.UseSkill(this, board);
        }
    }

}
