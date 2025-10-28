using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveController : MonoBehaviour
{
  public static Action<int> OnWaveChange;
  private static WaitForSeconds _waitForSeconds = new(.5f);
  private int currentWave = 1;

  [SerializeField] private WaveSpawner waveSpawner;
  [SerializeField] private BoardManager boardManager;
  [SerializeField] private PlayerManager playerManager;
  [SerializeField] private WaveListSO waveList;
  [SerializeField] private PickupManager pickupManager;

  // [SerializeField] private RewardManager rewardManager;
  // [SerializeField] private EnemyAttackSystem enemyAttackSystem;

  public IEnumerator RunWave(int waveNumber)
  {
    OnWaveChange?.Invoke(currentWave);

    // 1️⃣ Spawn
    yield return SpawnEnemiesPhase();

    // 2️⃣ Apply status effects
    yield return ApplyEnemyStatusPhase();

    // 3️⃣ Player shoot
    yield return PlayerShootPhase();

    // 4️⃣ Collect pickups
    yield return CollectPickupPhase();

    // 5️⃣ Monster attack
    yield return MonsterAttackPhase();

    // 6️⃣ Monster move
    yield return MonsterMovePhase();

    currentWave++;
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
    playerManager.EnableShooting(true);
    yield return new WaitUntil(() => playerManager.ShotAllBall);
    playerManager.EnableShooting(false);
  }

  private IEnumerator CollectPickupPhase()
  {
    Debug.Log("Collecting pickups...");

    if (pickupManager != null)
    {
      yield return pickupManager.ProcessAllPickups();
    }

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

  }


}
