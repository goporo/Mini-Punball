using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillCard : MonoBehaviour
{
  private int skillID;
  public TMP_Text skillName;
  public Sprite icon;
  public Button buttonCard;
  public Button buttonInfo;
  public event System.Action<int> OnSkillInfoClicked;
  public event System.Action<int> OnSkillSelected;

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