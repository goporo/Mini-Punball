using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MiniPunBall/Skill/PlayerSkillSO", order = -10)]
public class PlayerSkillSO : ScriptableObject
{
  [Header("Meta")]
  [SerializeField] private int skillID;
  public string skillName;
  [TextArea]
  public string description;
  public Sprite icon;
  public Rarity rarity;

  [Header("Core Components")]
  public List<TriggerSO> triggers;
  public List<ConditionSO> conditions;
  public List<EffectSO> effects;
  public int SkillID => skillID;


  [Header("Stacking")]
  public bool isStackable = false;
  public int maxStacks = 1;

  private void OnValidate()
  {
    // if (effects == null || effects.Count == 0)
    //   Debug.LogWarning($"[{name}] Skill has no effects!", this);

    if (isStackable && maxStacks < 1)
      maxStacks = 1;
  }


}

public enum Rarity
{
  Common,
  Rare,
  Epic,
  Legendary
}