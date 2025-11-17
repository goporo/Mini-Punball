using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(WaveController))]
public class LevelController : MonoBehaviour
{
    private WaveController waveController;
    [Header("Test Hardcoded Level Data")]
    [SerializeField] private LevelSO levelData;
    [SerializeField] private BoardManager boardManager;


    private int currentLevel = 1;
    private int currentWave = 1;
    public int CurrentLevel => currentLevel;
    public int CurrentWave => currentWave;
    private bool isLevelComplete = false;

    void Awake()
    {
        waveController = GetComponent<WaveController>();
        boardManager.InitBoard(levelData);
    }

    void OnEnable()
    {
        EventBus.Subscribe<OnHitEvent>(HandleOnEnemyHit);
        EventBus.Subscribe<OnPlayerDiedEvent>(HandleOnPlayerDied);
    }

    void OnDisable()
    {
        EventBus.Unsubscribe<OnHitEvent>(HandleOnEnemyHit);
        EventBus.Unsubscribe<OnPlayerDiedEvent>(HandleOnPlayerDied);
    }

    private void HandleOnPlayerDied(OnPlayerDiedEvent e)
    {
        // Optionally handle player death (e.g., stop level, show UI, etc.)
        Debug.Log("💀 Player died! Level failed.");
        StopAllCoroutines();
        OnLevelComplete(LevelResult.Lose);
    }

    private void HandleOnEnemyHit(OnHitEvent e)
    {
        if (e.killed && e.enemy.Data.Variant == EnemyVariant.Boss)
            OnLevelComplete(LevelResult.Win);
    }

    private void Start()
    {
        StartCoroutine(RunLevel());
    }

    private IEnumerator RunLevel()
    {
        while (true && !isLevelComplete)
        {
            Debug.Log($"▶️ Starting wave {currentWave}");
            yield return waveController.RunWave(currentLevel, currentWave, levelData);
            // Optionally: Wait for all enemies dead, pickups collected, etc.
            currentWave++;
        }
    }

    private void OnLevelComplete(LevelResult result)
    {
        isLevelComplete = true;
        StartCoroutine(WaitAndCompleteLevel(result));
    }

    private IEnumerator WaitAndCompleteLevel(LevelResult result)
    {
        yield return LevelContext.Instance.CoroutineCleanUp();
        yield return new WaitForSeconds(1f);
        EventBus.Publish(new LevelCompleteEvent(levelData, result));
        // Show summary, unlock next level, etc.
    }

    public void Toggle2xGameSpeed()
    {
        if (Time.timeScale == 1f)
        {
            Time.timeScale = 5f;
            return;
        }
        Time.timeScale = 1f;
    }
}

