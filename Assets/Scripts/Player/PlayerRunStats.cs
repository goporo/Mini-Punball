using System;
using UnityEngine;

public class PlayerRunStats : MonoBehaviour
{
  public PlayerStats Stats => playerStats;
  private PlayerStats playerStats;

  public int CurrentAttack => playerStats.Attack;
  public BallManager Balls;


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
    playerStats = new PlayerStats(
      GlobalContext.Instance.CharacterSO.BaseHealth,
      GlobalContext.Instance.CharacterSO.BaseAttack);

  }
  public void ApplyAttackBuff(float multiplier)
  {

  }



}

