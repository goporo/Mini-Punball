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

  private SkillManager skillManager;

  private void Awake()
  {
    skillManager = FindObjectOfType<SkillManager>();
  }

  void OnEnable()
  {
    LoadSkills();
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
    LoadSkills();
  }

  private void LoadSkills()
  {
    int count = 3;
    // Build a dictionary of current stack counts from SkillManager
    Dictionary<string, int> skillStacks = new Dictionary<string, int>();
    if (skillManager != null)
    {
      foreach (var runtime in skillManager.activeSkills)
      {
        skillStacks[runtime.skill.SkillID] = runtime.stackCount;
      }
    }

    List<PlayerSkillSO> availableSkills = new List<PlayerSkillSO>();
    foreach (var skill in skills.GetAllSkills())
    {
      int currentStack = skillStacks.ContainsKey(skill.SkillID) ? skillStacks[skill.SkillID] : 0;
      if (currentStack < skill.maxStacks)
        availableSkills.Add(skill);
      else
      {
        if (currentStack == 0)
          availableSkills.Add(skill);
      }
    }

    // Shuffle and pick up to 'count' unique skills
    int n = availableSkills.Count;
    for (int i = 0; i < n; i++)
    {
      int swapIdx = UnityEngine.Random.Range(i, n);
      var temp = availableSkills[i];
      availableSkills[i] = availableSkills[swapIdx];
      availableSkills[swapIdx] = temp;
    }

    int pickCount = Mathf.Min(count, availableSkills.Count);
    for (int i = 0; i < pickCount; i++)
    {
      PlayerSkillSO skill = availableSkills[i];
      GameObject skillCardObj = Instantiate(skillCardPrefab, skillCardContainer);
      SkillCard skillCard = skillCardObj.GetComponent<SkillCard>();
      skillCard.Init(skill);
      skillCard.OnSkillInfoClicked += OnClickSkillInfo;
      skillCard.OnSkillSelected += OnSkillSelect;
    }
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
      Debug.Log($"Skill selected: {playerSkillSO.skillName}");
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
