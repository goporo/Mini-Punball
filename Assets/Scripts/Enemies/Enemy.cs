using System.Collections;
using UnityEngine;
using System;
using DG.Tweening;

[RequireComponent(typeof(HealthComponent))]
public class Enemy : BoardObject, IAttacker
{
    [SerializeField] private EnemyUI enemyUI;

    [SerializeField] private EnemySO data;
    public WaveStats Stats;
    private HealthComponent healthComponent;
    public HealthComponent HealthComponent => healthComponent;
    private EnemySkillBehavior EnemySkillBehavior => data.enemySkillBehavior;
    private DeathEffect DeathEffect => data.deathEffect;
    private BoardState board;



    public IEnumerator AnimateSpawn(BoardState board)
    {
        yield return AnimateSpawn(this, board);
    }
    private int waveHealth;
    private int waveAttack;

    public void Init(float hpMultiplier, float attackMultiplier, BoardState board)
    {
        waveHealth = Mathf.CeilToInt(data.baseHealth * hpMultiplier);
        waveAttack = Mathf.CeilToInt(data.baseAttack * attackMultiplier);
        enemyUI?.Init(waveHealth);
        healthComponent.Init(waveHealth);
        Stats = new WaveStats(waveHealth, waveAttack);
        this.board = board;
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
            yield return EnemySkillBehavior.AttackAndDie(this, new DamageContext { amount = waveAttack });
        }
        else
        {
            yield return EnemySkillBehavior.UseSkill(this, board);
        }
    }

    public override void HandleOnDeath()
    {
        EventBus.Publish(new EnemyDeathEvent { Context = new BallHitContext(this, 69, null) });
        DeathEffect.OnDeath(this, board);
        base.HandleOnDeath();
    }

    public struct WaveStats
    {
        public int Health;
        public int Attack;
        public WaveStats(int health, int attack)
        {
            Health = health;
            Attack = attack;
        }
    }

}
