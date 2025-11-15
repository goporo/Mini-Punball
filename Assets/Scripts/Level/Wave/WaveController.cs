using System;
using System.Collections;
using UnityEngine;

public class WaveController : MonoBehaviour
{
  public enum WaveState
  {
    Idle,
    Spawning,
    ApplyingStatus,
    PlayerShooting,
    CollectingPickups,
    MonsterAttacking,
    MonsterMoving,
    Complete
  }

  public static Action<WaveState> OnStateChange;

  private static WaitForSeconds waitABit = new(.4f);

  [SerializeField] private WaveSpawner waveSpawner;
  [SerializeField] private BoardManager boardManager;
  [SerializeField] private PlayerManager playerManager;
  [SerializeField] private WaveListSO waveList;

  private WaveState _currentState = WaveState.Idle;
  private int currentLevelNumber;
  private int currentWaveNumber;
  private int totalWaves;

  private LevelSO levelData;
  public WaveState CurrentState => _currentState;


  public IEnumerator RunWave(int levelNumber, int waveNumber, LevelSO levelData)
  {
    currentLevelNumber = levelNumber;
    currentWaveNumber = waveNumber;
    this.levelData = levelData;
    this.totalWaves = levelData.TotalWaves;
    if (waveNumber == totalWaves - 1)
    {
      Debug.Log("[WaveController] Boss wave approaching!");
      EventBus.Publish(new OnBossWaveApproachingEvent());
    }
    else if (waveNumber == totalWaves)
    {
      EventBus.Publish(new OnBossWaveStartEvent(levelData.BossLists[levelNumber - 1].bossEnemy));
    }


    yield return ExecuteStateMachine();
  }

  private IEnumerator ExecuteStateMachine()
  {
    yield return waitABit;
    yield return ChangeState(WaveState.Spawning);
    yield return ChangeState(WaveState.ApplyingStatus);
    yield return ChangeState(WaveState.PlayerShooting);
    yield return ChangeState(WaveState.CollectingPickups);
    yield return ChangeState(WaveState.MonsterAttacking);
    yield return ChangeState(WaveState.MonsterMoving);
    yield return ChangeState(WaveState.Complete);
  }

  private IEnumerator ChangeState(WaveState newState)
  {
    _currentState = newState;
    OnStateChange?.Invoke(_currentState);

    Debug.Log($"[WaveController] State Changed: {_currentState}");

    yield return ExecuteState(_currentState);
  }

  private IEnumerator ExecuteState(WaveState state)
  {
    switch (state)
    {
      case WaveState.Spawning:
        yield return SpawnEnemiesPhase(currentLevelNumber, currentWaveNumber);
        break;

      case WaveState.ApplyingStatus:
        yield return ApplyEnemyStatusPhase();
        break;

      case WaveState.PlayerShooting:
        yield return PlayerShootPhase();
        break;

      case WaveState.CollectingPickups:
        yield return CollectPickupPhase();
        break;

      case WaveState.MonsterAttacking:
        yield return MonsterAttackPhase();
        break;

      case WaveState.MonsterMoving:
        yield return MonsterMovePhase();
        break;

      case WaveState.Complete:
        Debug.Log("[WaveController] Wave Complete!");
        break;

      default:
        Debug.LogWarning($"[WaveController] Unknown state: {state}");
        break;
    }
  }

  private IEnumerator SpawnEnemiesPhase(int levelNumber, int waveNumber)
  {
    Enemy[] availableEnemies = levelData.availableEnemies;
    BossList[] bossLists = levelData.BossLists;
    string waveText = waveNumber >= totalWaves ? "Boss" : $"{waveNumber}";
    EventBus.Publish(new OnWaveStartEvent(levelNumber, waveNumber, waveText, availableEnemies));
    yield return waveSpawner.SpawnWave(waveList.GenerateWave(levelNumber, waveNumber, availableEnemies, bossLists, totalWaves));
  }

  private IEnumerator ApplyEnemyStatusPhase()
  {
    yield return waitABit;
    // TODO: Add VFX waiting here if needed
    // yield return WaitForVFX("StatusEffectVFX");
  }

  private IEnumerator PlayerShootPhase()
  {
    yield return playerManager.StartShooting();
    yield return WaitAllEffectsFinished();
  }

  private IEnumerator CollectPickupPhase()
  {
    yield return LevelContext.Instance.PickupManager.ProcessAllCollects();
    // TODO: Add VFX waiting here if needed
    // yield return WaitForVFX("PickupVFX");
  }

  private IEnumerator MonsterAttackPhase()
  {
    yield return boardManager.StartAttack();
    yield return WaitAllEffectsFinished();
  }

  private IEnumerator MonsterMovePhase()
  {
    yield return boardManager.StartMove();
    // TODO: Add VFX waiting here if needed
    // yield return WaitForVFX("MoveVFX");
  }
  private IEnumerator WaitAllEffectsFinished()
  {
    Debug.Log("Waiting for all effects to finish...");
    yield return new WaitUntil(() => LevelContext.Instance.VFXManager.AllEffectsFinished());
    yield return waitABit;

  }

  /// <summary>
  /// Manually transition to a specific state (useful for external control or skipping states)
  /// </summary>
  public void ForceStateTransition(WaveState targetState)
  {
    Debug.Log($"[WaveController] Force state transition to: {targetState}");
    StartCoroutine(ChangeState(targetState));
  }
}
