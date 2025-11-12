using System;
using UnityEngine;

public class PlayerRunStats : MonoBehaviour
{
  public PlayerStats Stats => playerStats;
  private PlayerStats baseStats;
  private PlayerStats playerStats;

  public int CurrentAttack => playerStats.Attack;
  public int CurrentHealth => playerStats.Health;
  public BallManager Balls;
  public HealthComponent HealthComponent;
  public Vector3 Position => transform.position + Vector3.up * 1.0f;


  public class PlayerStats
  {
    public int Health { get; set; }
    public int Attack { get; set; }

    public PlayerStats(int health, int attack)
    {
      Health = health;
      Attack = attack;
    }
  }

  void Awake()
  {
    baseStats = new PlayerStats(
      GlobalContext.Instance.CharacterSO.BaseHealth,
      GlobalContext.Instance.CharacterSO.BaseAttack);

    playerStats = baseStats;
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
  public void ApplyAttackBuff(float multiplier)
  {
    playerStats.Attack = (int)(baseStats.Attack * multiplier);
  }

  public void ApplyHealthBuff(float multiplier)
  {
    playerStats.Health = (int)(baseStats.Health * multiplier);
    HealthComponent.Init(playerStats.Health);
  }



}

