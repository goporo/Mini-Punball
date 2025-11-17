using System.Collections;
using UnityEngine;
using System;
using DG.Tweening;

[RequireComponent(typeof(HealthComponent))]
public class Enemy : BoardObject, IAttacker
{
    [SerializeField] private EnemyUI enemyUI;

    [SerializeField] private EnemySO data;
    public EnemySO Data => data;
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
    private int baseHealth;
    private int baseAttack;

    public void Init(float hpMultiplier, float attackMultiplier, BoardState board)
    {
        baseHealth = Mathf.CeilToInt(data.baseHealth * hpMultiplier);
        baseAttack = Mathf.CeilToInt(data.baseAttack * attackMultiplier);
        enemyUI?.Init(baseHealth);
        healthComponent.Init(baseHealth);
        Stats = new WaveStats(baseHealth, baseAttack);
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
            yield return EnemySkillBehavior.AttackAndDie(this, new PlayerDamageContext { FinalDamage = baseAttack });
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
