using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunStats : MonoBehaviour
{
  private int currentAttack;
  public List<BallSO> CurrentBalls = new();

  public event Action OnStatsChanged;
  public int CurrentAttack => currentAttack;

  void Awake()
  {
    currentAttack = GameManager.Instance.CharacterSO.BaseAttack;
  }
  public void ApplyAttackBuff(float multiplier)
  {
    currentAttack = (int)(currentAttack * multiplier);
    OnStatsChanged?.Invoke();
  }


  public void AddBall(BallSO newBall)
  {
    CurrentBalls.Add(newBall);
  }

}
