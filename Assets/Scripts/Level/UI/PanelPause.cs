using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class PanelPause : MonoBehaviour
{
  [SerializeField] private Button buttonClose;
  [SerializeField] private Button buttonQuit;

  [SerializeField] private GameObject skillListContainer;
  [SerializeField] private GameObject skillIconPrefab;
  [SerializeField] private GameObject popupInfo;
  [SerializeField] private TMP_Text popupSkillName;
  [SerializeField] private TMP_Text popupSkillDescription;

  void Awake()
  {
    gameObject.SetActive(false);
    buttonClose.onClick.AddListener(HidePausePanel);
    buttonQuit.onClick.AddListener(QuitLevel);
  }

  void OnEnable()
  {
    popupInfo.SetActive(false);
  }

  private void InitSkills()
  {
    var currentSkills = LevelContext.Instance.SkillManager.GetActiveSkills();
    foreach (Transform child in skillListContainer.transform)
    {
      Destroy(child.gameObject);
    }
    foreach (var skill in currentSkills)
    {
      var skillCardObj = Instantiate(skillIconPrefab, skillListContainer.transform);
      var skillCardMini = skillCardObj.GetComponent<SkillCardMini>();
      skillCardMini.Init(skill);
      skillCardMini.OnSkillCardClicked += OnClickSkillInfo;
    }
  }

  public void OnClickSkillInfo(string skillId)
  {
    PlayerSkillSO playerSkillSO = GlobalContext.Instance.skills.GetSkillByID(skillId);
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

  private void QuitLevel()
  {
    Time.timeScale = 1f;
    GlobalContext.Instance.LoadScene(MainScenes.MenuScene);
  }


  public void ShowPausePanel()
  {
    Time.timeScale = 0f;
    gameObject.SetActive(true);
    InitSkills();
  }

  public void HidePausePanel()
  {
    Time.timeScale = 1f;
    gameObject.SetActive(false);
  }
}