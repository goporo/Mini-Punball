using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillCard : MonoBehaviour
{
  private string skillID;
  public TMP_Text skillName;
  public Image icon;
  public Button buttonCard;
  public Button buttonInfo;
  public event System.Action<string> OnSkillInfoClicked;
  public event System.Action<string> OnSkillSelected;

  void OnEnable()
  {
    buttonCard.onClick.AddListener(OnCardClick);
    buttonInfo.onClick.AddListener(OnInfoClick);
  }

  void OnDisable()
  {
    buttonCard.onClick.RemoveListener(OnCardClick);
    buttonInfo.onClick.RemoveListener(OnInfoClick);
  }


  public void Init(PlayerSkillSO playerSkillSO)
  {
    skillID = playerSkillSO.SkillID;
    skillName.text = playerSkillSO.skillName;
    if (playerSkillSO.icon) icon.sprite = playerSkillSO.icon;
  }

  public void OnCardClick()
  {
    OnSkillSelected?.Invoke(skillID);
  }


  public void OnInfoClick()
  {
    OnSkillInfoClicked?.Invoke(skillID);
  }
}