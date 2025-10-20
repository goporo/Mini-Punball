using System;
using UnityEngine;
using DG.Tweening;

public class PlayerProfile : SingletonPersist<PlayerProfile>
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
}