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
        int wave = 1;
        while (true)
        {
            Debug.Log($"▶️ Starting wave {wave}");
            yield return waveController.RunWave(currentLevel, wave, levelData);
            // Optionally: Wait for all enemies dead, pickups collected, etc.
            wave++;
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

