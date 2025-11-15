using System;
using UnityEngine;


public enum MainScenes
{
  MenuScene,
  GamePlayScene
}

public class GlobalContext : MonoBehaviour
{
  public static GlobalContext Instance;
  [SerializeField] CharacterSO characterSO;

  public CharacterSO CharacterSO => characterSO;

  void Awake()
  {
    if (Instance == null)
    {
      Instance = this;
      DontDestroyOnLoad(gameObject);
    }
    else
    {
      Destroy(gameObject); // Prevent duplicates
    }
  }

  void Start()
  {
    ApplyUpgrade();
  }

  void ApplyUpgrade()
  {
    Debug.Log("Applied player upgrade");
  }

  public void LoadScene(string sceneName)
  {
    UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
  }

  public void LoadScene(MainScenes scene)
  {
    LoadScene(scene.ToString());
  }
}