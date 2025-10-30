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

  private void Awake()
  {
    popupInfo.SetActive(false);
  }

  void OnEnable()
  {
    LoadSkills();
    buttonResetSkills.onClick.AddListener(OnResetSkills);
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
    List<PlayerSkillSO> randomSkills = skills.GetRandomSkills(3, null);
    foreach (PlayerSkillSO skill in randomSkills)
    {
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

  public void OnSkillSelect(int skillId)
  {
    PlayerSkillSO playerSkillSO = skills.GetSkillByID(skillId);
    if (playerSkillSO != null)
    {
      Debug.Log($"Skill selected: {playerSkillSO.skillName}");
      EventBus.Publish(new SkillSelectedEvent(playerSkillSO));
      // Close the UI and notify that selection is complete
      gameObject.SetActive(false);
      EventBus.Publish(new SkillSelectionCompleteEvent());
    }
    else
    {
      Debug.LogWarning($"SkillUI: Skill with ID {skillId} not found.");
    }
  }
  public void OnClickSkillInfo(int skillId)
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
