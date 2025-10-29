using System;
using UnityEngine;
using DG.Tweening;

public class GameManager : SingletonPersist<GameManager>
{
  [SerializeField] CharacterSO characterSO;

  public CharacterSO CharacterSO => characterSO;

  protected override void Awake()
  {
    base.Awake();
    ApplyUpgrade();
  }

  void ApplyUpgrade()
  {
    int level = 1;
    characterSO.BaseAttack *= level;
    Debug.Log("Applied player upgrade");
  }

  public void LoadScene(string sceneName)
  {
    UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
  }
}