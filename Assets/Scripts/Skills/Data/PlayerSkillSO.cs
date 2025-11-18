using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MiniPunBall/Skill/PlayerSkillSO", order = -10)]
public class PlayerSkillSO : ScriptableObject
{
  [SerializeField, HideInInspector]
  private string skillId;
  public string SkillID => skillId;


  [Header("Meta")]
  public string skillName;
  [TextArea]
  public string description;
  public Sprite icon;
  public Rarity rarity;


  [Header("Core Components")]
  public List<TriggerSO> triggers = new();
  public List<ConditionSO> conditions = new();
  public List<EffectSO> effects = new();
  public int maxStacks = 1;

  private void OnValidate()
  {
    if (string.IsNullOrEmpty(skillId))
    {
      skillId = Guid.NewGuid().ToString();
#if UNITY_EDITOR
      UnityEditor.EditorUtility.SetDirty(this);
#endif
    }

  }
}

public enum Rarity
{
  Common,
  Rare,
  Epic,
  Legendary
}
