using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PickupManager))]
public class WaveController : MonoBehaviour
{
  public static Action<int> OnWaveChange;
  private static WaitForSeconds _waitForSeconds = new(.5f);

  [SerializeField] private WaveSpawner waveSpawner;
  [SerializeField] private BoardManager boardManager;
  [SerializeField] private PlayerManager playerManager;
  [SerializeField] private WaveListSO waveList;
  private PickupManager pickupManager;

  private void Awake()
  {
    pickupManager = GetComponent<PickupManager>();
  }

  public IEnumerator RunWave(int levelNumber, int waveNumber)
  {
    OnWaveChange?.Invoke(waveNumber);

    // 1️⃣ Spawn
    yield return SpawnEnemiesPhase(levelNumber, waveNumber);

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
  }

  private IEnumerator SpawnEnemiesPhase(int levelNumber, int waveNumber)
  {
    Debug.Log($"Spawning wave {waveNumber} for level {levelNumber}...");
    yield return waveSpawner.SpawnWave(waveList.GenerateWave(levelNumber, waveNumber));
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
    yield return playerManager.StartShooting();
  }

  private IEnumerator CollectPickupPhase()
  {
    Debug.Log("Collecting pickups...");
    yield return pickupManager.ProcessAllPickups();

  }

  private IEnumerator MonsterAttackPhase()
  {
    Debug.Log("Enemies attacking...");
    yield return boardManager.StartAttack();
    // yield return enemyAttackSystem.ExecuteAttacks();
  }

  private IEnumerator MonsterMovePhase()
  {
    Debug.Log("Enemies moving...");
    yield return boardManager.StartMove();

  }


}
