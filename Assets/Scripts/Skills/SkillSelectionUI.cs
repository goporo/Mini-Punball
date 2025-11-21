using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillSelectionUI : MonoBehaviour
{
  public GameObject popupInfo;
  public TMP_Text popupSkillName;
  public TMP_Text popupSkillDescription;
  public SkillDatabaseSO skills;
  public GameObject skillCardPrefab;
  public Transform skillCardContainer;
  public Button buttonResetSkills;
  private List<PlayerSkillSO> currentSkills;

  private SkillManager skillManager => LevelContext.Instance.SkillManager;

  void OnEnable()
  {
    currentSkills = LoadSkills();
    buttonResetSkills.onClick.AddListener(OnResetSkills);
    popupInfo.SetActive(false);
  }

  void OnDisable()
  {
    ClearSkills();
    buttonResetSkills.onClick.RemoveListener(OnResetSkills);
  }

  private void OnResetSkills()
  {
    ClearSkills();
    currentSkills = LoadSkills(currentSkills);
  }

  private List<PlayerSkillSO> LoadSkills(List<PlayerSkillSO> excludeSkills = null)
  {
    int count = 3;
    List<PlayerSkillSO> availableSkills = new List<PlayerSkillSO>();
    HashSet<string> excludeSkillIds = null;
    if (excludeSkills != null)
    {
      excludeSkillIds = new HashSet<string>();
      foreach (var exSkill in excludeSkills)
      {
        if (exSkill != null)
          excludeSkillIds.Add(exSkill.SkillID);
      }
    }
    foreach (var skill in skills.GetAllSkills())
    {
      int currentStack = 0;
      if (skillManager != null)
      {
        var runtime = skillManager.activeSkills.Find(s => s.skill.SkillID == skill.SkillID);
        if (runtime != null)
          currentStack = runtime.stackCount;
      }
      bool isExcluded = excludeSkillIds != null && excludeSkillIds.Contains(skill.SkillID);
      if (currentStack < skill.maxStacks && !isExcluded)
        availableSkills.Add(skill);
    }

    // Shuffle and pick up to 'count' unique skills
    int n = availableSkills.Count;
    for (int i = 0; i < n; i++)
    {
      int swapIdx = UnityEngine.Random.Range(i, n);
      (availableSkills[swapIdx], availableSkills[i]) = (availableSkills[i], availableSkills[swapIdx]);
    }

    int pickCount = Mathf.Min(count, availableSkills.Count);
    List<PlayerSkillSO> shownSkills = new List<PlayerSkillSO>();
    for (int i = 0; i < pickCount; i++)
    {
      PlayerSkillSO skill = availableSkills[i];
      GameObject skillCardObj = Instantiate(skillCardPrefab, skillCardContainer);
      SkillCard skillCard = skillCardObj.GetComponent<SkillCard>();
      skillCard.Init(skill);
      skillCard.OnSkillInfoClicked += OnClickSkillInfo;
      skillCard.OnSkillSelected += OnSkillSelect;
      shownSkills.Add(skill);
    }
    return shownSkills;
  }

  private void ClearSkills()
  {
    foreach (Transform child in skillCardContainer)
    {
      SkillCard skillCard = child.GetComponent<SkillCard>();
      if (skillCard != null)
      {
        skillCard.OnSkillInfoClicked -= OnClickSkillInfo;
        skillCard.OnSkillSelected -= OnSkillSelect;
      }
      Destroy(child.gameObject);
    }
  }

  public void OnSkillSelect(string skillId)
  {
    PlayerSkillSO playerSkillSO = skills.GetSkillByID(skillId);
    if (playerSkillSO != null)
    {
      var ctx = new EffectContext(null);
      EventBus.Publish(new SkillSelectedEvent(ctx, playerSkillSO));
      // Close the UI and notify that selection is complete
      gameObject.SetActive(false);
      EventBus.Publish(new SkillSelectionCompleteEvent());
    }
    else
    {
      Debug.LogWarning($"SkillUI: Skill with ID {skillId} not found.");
    }
  }
  public void OnClickSkillInfo(string skillId)
  {
    PlayerSkillSO playerSkillSO = skills.GetSkillByID(skillId);
    if (playerSkillSO != null)
    {
      popupSkillName.text = playerSkillSO.skillName;
      popupSkillDescription.text = playerSkillSO.description;
      popupInfo.SetActive(true);
    }
    else
    {
      Debug.LogWarning($"SkillUI: Skill with ID {skillId} not found.");
    }
  }
}
