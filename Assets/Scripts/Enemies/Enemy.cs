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
    private int waveHealth;
    private int waveAttack;

    public void Init(float hpMultiplier, float attackMultiplier)
    {
        waveHealth = Mathf.CeilToInt(data.baseHealth * hpMultiplier);
        waveAttack = Mathf.CeilToInt(data.baseAttack * attackMultiplier);
        enemyUI?.Init(waveHealth);
        healthComponent.Init(waveHealth);
    }

    private void Awake()
    {
        healthComponent = GetComponent<HealthComponent>();
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
            yield return skillBehavior.AttackAndDie(this, new DamageContext { amount = waveAttack });
        }
        else
        {
            yield return skillBehavior.UseSkill(this, board);
        }
    }

}
