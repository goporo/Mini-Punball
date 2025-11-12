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

    void Awake()
    {
        waveController = GetComponent<WaveController>();
        boardManager.InitBoard(levelData);
    }

    private void Start()
    {
        StartCoroutine(RunLevel());
    }

    private IEnumerator RunLevel()
    {
        while (true)
        {
            Debug.Log($"▶️ Starting wave {currentWave}");
            yield return waveController.RunWave(currentLevel, currentWave, levelData);
            // Optionally: Wait for all enemies dead, pickups collected, etc.
            currentWave++;
        }
    }

    private void OnLevelComplete()
    {
        Debug.Log("🏁 Level complete!");
        // Show summary, unlock next level, etc.
    }

    public void Toggle2xGameSpeed()
    {
        if (Time.timeScale == 1f)
        {
            Time.timeScale = 3f;
            return;
        }
        Time.timeScale = 1f;
    }
}

