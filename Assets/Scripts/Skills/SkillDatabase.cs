using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillDatabaseSO", menuName = "MiniPunBall/SkillDatabaseSO", order = 0)]
public class SkillDatabaseSO : ScriptableObject
{
  public PlayerSkillSO[] skills;

  public PlayerSkillSO GetSkillByID(int id)
  {
    foreach (var skill in skills)
    {
      if (skill.skillID == id)
      {
        return skill;
      }
    }
    Debug.LogWarning($"Skill with ID {id} not found in SkillDatabaseSO.");
    return null;
  }

  public List<PlayerSkillSO> GetRandomSkills(int count, List<int> excludeIDs = null)
  {
    excludeIDs ??= new List<int>();
    List<PlayerSkillSO> availableSkills = new List<PlayerSkillSO>();
    foreach (var skill in skills)
    {
      if (!excludeIDs.Contains(skill.skillID))
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