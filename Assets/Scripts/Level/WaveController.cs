using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveController : MonoBehaviour
{
  public static Action<int> OnWaveChange;
  private static WaitForSeconds _waitForSeconds = new WaitForSeconds(.5f);
  private int currentWave = 1;

  [SerializeField] private WaveSpawner waveSpawner;
  [SerializeField] private BoardManager boardManager;
  [SerializeField] private PlayerManager playerManager;
  [SerializeField] private WaveListSO waveList;

  // [SerializeField] private RewardManager rewardManager;
  // [SerializeField] private PickupManager pickupManager;
  // [SerializeField] private EnemyAttackSystem enemyAttackSystem;

  private bool waveRunning;

  public IEnumerator RunWave(int waveNumber)
  {
    OnWaveChange?.Invoke(currentWave);
    waveRunning = true;

    // 1️⃣ Spawn
    yield return SpawnEnemiesPhase();

    // 2️⃣ Apply status effects
    yield return ApplyEnemyStatusPhase();

    // 3️⃣ Player shoot
    yield return PlayerShootPhase();

    // 4️⃣ Reward selection
    yield return RewardPhase();

    // 5️⃣ Collect pickups
    yield return CollectPickupPhase();

    // 6️⃣ Monster attack
    yield return MonsterAttackPhase();

    // 7️⃣ Monster move
    yield return MonsterMovePhase();

    currentWave++;
    waveRunning = false;
  }

  private IEnumerator SpawnEnemiesPhase()
  {
    Debug.Log("Spawning wave...");
    yield return waveSpawner.SpawnWave(waveList.GenerateWave(currentWave));
  }

  private IEnumerator ApplyEnemyStatusPhase()
  {
    Debug.Log("Applying enemy status...");
    yield return _waitForSeconds;
    // yield return enemyManager.ApplyStatusEffects();
  }

  private IEnumerator PlayerShootPhase()
  {
    Debug.Log("Player shooting...");
    bool done = false;
    void handler(List<BallBase> balls) => done = true;
    BallManager.OnAllBallsReturned += handler;

    LevelRuntimeData.Instance.CanShoot = true;
    yield return new WaitUntil(() => done);
    LevelRuntimeData.Instance.CanShoot = false;

    BallManager.OnAllBallsReturned -= handler;
  }

  private IEnumerator RewardPhase()
  {
    Debug.Log("Reward phase...");
    yield return _waitForSeconds;
  }

  private IEnumerator CollectPickupPhase()
  {
    Debug.Log("Collecting pickups...");
    yield return _waitForSeconds;
    // yield return pickupManager.CollectAll();
  }

  private IEnumerator MonsterAttackPhase()
  {
    Debug.Log("Enemies attacking...");
    yield return _waitForSeconds;
    // yield return enemyAttackSystem.ExecuteAttacks();
  }

  private IEnumerator MonsterMovePhase()
  {
    Debug.Log("Enemies moving...");
    yield return boardManager.StartMove();
    // yield return board.MoveAllEnemiesForward();

  }


  public bool IsWaveRunning => waveRunning;





}
