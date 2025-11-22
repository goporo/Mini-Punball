using System.Collections;
using UnityEngine;

[RequireComponent(typeof(HealthComponent))]
public class Enemy : BoardObject, IAttacker
{
    [SerializeField] private EnemyUI enemyUI;

    [SerializeField] private EnemySO data;
    public EnemySO Stats => data;
    public WaveStatus WaveStats;
    private HealthComponent healthComponent;
    public HealthComponent HealthComponent => healthComponent;
    private EnemySkillBehavior EnemySkillBehavior => data.enemySkillBehavior;
    private DeathEffect DeathEffect => data.deathEffect;
    private BoardState board;
    public StatusController StatusController { get; private set; } = null;
    public EnemyStatusVisuals EnemyStatusVisuals { get; private set; } = null;


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
        WaveStats = new WaveStatus(baseHealth, baseAttack);
        this.board = board;
    }

    private void Awake()
    {
        healthComponent = GetComponent<HealthComponent>();
        StatusController = new StatusController(this);
        EnemyStatusVisuals = GetComponentInChildren<EnemyStatusVisuals>();
    }

    void OnEnable()
    {
        healthComponent.OnDied += HandleOnDeath;
    }

    void OnDisable()
    {
        healthComponent.OnDied -= HandleOnDeath;
    }

    public void ApplyStatusEffect(StatusEffectBase effect)
    {
        StatusController.AddEffect(effect);
    }


    public IEnumerator DoAttack(BoardState board)
    {
        if (!CanAct) yield break;

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
        EventBus.Publish(new EnemyDeathEvent { Context = new EnemyDeathContext(this) });
        DeathEffect.OnDeath(this, board);
        base.HandleOnDeath();
    }

    public struct WaveStatus
    {
        public int Health;
        public int Attack;
        public WaveStatus(int health, int attack)
        {
            Health = health;
            Attack = attack;
        }
    }

}
