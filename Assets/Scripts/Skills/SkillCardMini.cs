using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillCardMini : MonoBehaviour
{
  private string skillID;
  public Image icon;
  public Button buttonCard;
  public event System.Action<string> OnSkillCardClicked;

  void OnEnable()
  {
    buttonCard.onClick.AddListener(OnCardClick);
  }

  void OnDisable()
  {
    buttonCard.onClick.RemoveListener(OnCardClick);
  }

  public void Init(PlayerSkillSO playerSkillSO)
  {
    skillID = playerSkillSO.SkillID;
    if (playerSkillSO.icon) icon.sprite = playerSkillSO.icon;
  }

  public void OnCardClick()
  {
    OnSkillCardClicked?.Invoke(skillID);

  }

}