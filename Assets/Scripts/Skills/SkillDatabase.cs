using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MiniPunBall/SkillDatabaseSO")]
public class SkillDatabaseSO : ScriptableObject
{
  public PlayerSkillSO[] skills;
  public PlayerSkillSO GetSkillByID(string id)
  {
    foreach (var skill in skills)
    {
      if (skill.SkillID == id)
      {
        return skill;
      }
    }
    Debug.LogWarning($"Skill with ID {id} not found in SkillDatabaseSO.");
    return null;
  }

  public List<PlayerSkillSO> GetAllSkills()
  {
    return new List<PlayerSkillSO>(skills);
  }

  public List<PlayerSkillSO> GetRandomSkills(int count, List<string> excludeIDs = null)
  {
    excludeIDs ??= new List<string>();
    List<PlayerSkillSO> availableSkills = new List<PlayerSkillSO>();
    foreach (var skill in skills)
    {
      if (!excludeIDs.Contains(skill.SkillID))
      {
        availableSkills.Add(skill);
      }
    }

    List<PlayerSkillSO> selectedSkills = new List<PlayerSkillSO>();
    int selectionCount = Mathf.Min(count, availableSkills.Count);
    for (int i = 0; i < selectionCount; i++)
    {
      int randomIndex = Random.Range(0, availableSkills.Count);
      selectedSkills.Add(availableSkills[randomIndex]);
      availableSkills.RemoveAt(randomIndex);
    }

    return selectedSkills;
  }

}