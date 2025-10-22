using System;
using System.Collections.Generic;
using UnityEngine;
public class LevelStateMachine
{
  public LevelState Current { get; private set; } = LevelState.None;
  public event Action<LevelState, LevelState> OnStateChanged;

  private static readonly Dictionary<LevelState, LevelState> NextMap = new()
    {
        { LevelState.None, LevelState.Loading },
        { LevelState.Loading, LevelState.SpawningEnemies },
        { LevelState.SpawningEnemies, LevelState.ApplyEnemyStatus },
        { LevelState.ApplyEnemyStatus, LevelState.PlayerShoot },
        { LevelState.PlayerShoot, LevelState.ChooseReward },
        { LevelState.ChooseReward, LevelState.CollectPickup },
        { LevelState.CollectPickup, LevelState.MonsterAttack },
        { LevelState.MonsterAttack, LevelState.MonsterMove },
        { LevelState.MonsterMove, LevelState.SpawningEnemies },
    };

  public void NextState()
  {
    if (NextMap.TryGetValue(Current, out var next))
      SetState(next);
    else
      Debug.LogWarning($"No next state after {Current}");
  }

  public void SetState(LevelState newState)
  {
    var prev = Current;
    Current = newState;
    OnStateChanged?.Invoke(prev, newState);
  }
}

public enum LevelState
{
  None,
  Loading,
  SpawningEnemies,
  ApplyEnemyStatus,
  PlayerShoot,
  ChooseReward,
  CollectPickup,
  MonsterAttack,
  MonsterMove
}
