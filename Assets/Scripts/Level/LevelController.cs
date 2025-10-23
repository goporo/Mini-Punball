using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WaveController))]
public class LevelController : MonoBehaviour
{
    private WaveController waveController;
    [Header("Test Hardcoded Level Data")]
    [SerializeField] private LevelSO levelData;
    [SerializeField] private BoardManager boardManager;



    private int currentWave = 0;

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
        while (currentWave < levelData.totalWaves)
        {
            Debug.Log($"▶️ Starting wave {currentWave + 1}");
            yield return waveController.RunWave(1);

            // Wait until enemies are all dead before moving on

            currentWave++;
        }

        OnLevelComplete();
    }

    private void OnLevelComplete()
    {
        Debug.Log("🏁 Level complete!");
        // Show summary, unlock next level, etc.
    }
}

