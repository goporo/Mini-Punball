using UnityEngine;

public class PlayerRunStats : MonoBehaviour
{
  private PlayerStats baseStats;       // from character SO
  private PlayerStats permanentStats;  // permanent per-run modifiers
  private PlayerStats roundStats;      // changes every round
  private PlayerStats playerStats;     // final computed stats
  private RunBuffs runBuffs;

  public int CurrentAttack => playerStats.Attack;
  public int CurrentHealth => playerStats.Health;

  public BallManager Balls;
  public HealthComponent HealthComponent;
  public Vector3 Position => transform.position + Vector3.up;
  [SerializeField] private PlayerController playerController;

  [System.Serializable]
  public struct PlayerStats
  {
    public int Health;
    public int Attack;

    public PlayerStats(int health, int attack)
    {
      Health = health;
      Attack = attack;
    }

    public static PlayerStats operator +(PlayerStats a, PlayerStats b) =>
        new PlayerStats(a.Health + b.Health, a.Attack + b.Attack);

    public static PlayerStats operator *(PlayerStats a, float m) =>
        new PlayerStats(Mathf.RoundToInt(a.Health * m), Mathf.RoundToInt(a.Attack * m));
  }

  [System.Serializable]
  public struct RunBuffs
  {
    public float HealPickupMultiplier;
  }

  void Awake()
  {
    baseStats = new PlayerStats(
        GlobalContext.Instance.CharacterSO.BaseHealth,
        GlobalContext.Instance.CharacterSO.BaseAttack);

    permanentStats = new PlayerStats(0, 0);
    roundStats = new PlayerStats(0, 0);
    runBuffs = new RunBuffs { HealPickupMultiplier = 1f };

    Recalculate();
    HealthComponent.Init(playerStats.Health);
  }

  void OnEnable()
  {
    EventBus.Subscribe<PickupHealthEvent>(OnPickupHealth);
  }

  void OnDisable()
  {
    EventBus.Unsubscribe<PickupHealthEvent>(OnPickupHealth);
  }

  private void OnPickupHealth(PickupHealthEvent e)
  {
    ApplyHeal(e.Amount);
  }

  public void ApplyHeal(int amount)
  {
    HealthComponent.Heal(amount);
  }

  // ————————— BUFFS —————————

  public void ApplyAttackBuff(float multiplier)
  {
    permanentStats.Attack += Mathf.RoundToInt(baseStats.Attack * (multiplier - 1));
    Recalculate();
  }

  public void ApplyHealthBuff(float multiplier)
  {
    permanentStats.Health += Mathf.RoundToInt(baseStats.Health * (multiplier - 1));
    Recalculate();
    HealthComponent.SetMaxHealth(playerStats.Health);
  }

  public void ApplyHealPickupBuff(float multiplier)
  {
    runBuffs.HealPickupMultiplier *= multiplier;

  }

  public int GetModifiedHealAmount(int baseHeal)
  {
    return Mathf.RoundToInt(baseHeal * runBuffs.HealPickupMultiplier);
  }

  public void ApplyInvincibleBuff(int durationRounds)
  {
    HealthComponent.ApplyInvincible(durationRounds);
  }

  private void Recalculate()
  {
    playerStats = baseStats + permanentStats + roundStats;
  }
}
