using UnityEngine;

public class SkillManager : MonoBehaviour
{
  public GameObject skillSelectionUI;


  private void Awake()
  {
    skillSelectionUI.SetActive(false);
  }

  private void OnEnable()
  {
    EventBus.Subscribe<SkillSelectedEvent>(OnSkillSelected);
    EventBus.Subscribe<PickupBoxEvent>(OnPickupBoxEvent);
  }

  private void OnDisable()
  {
    EventBus.Unsubscribe<SkillSelectedEvent>(OnSkillSelected);
    EventBus.Unsubscribe<PickupBoxEvent>(OnPickupBoxEvent);
  }

  private void OnPickupBoxEvent(PickupBoxEvent e)
  {
    skillSelectionUI.SetActive(true);
  }


  private void OnSkillSelected(SkillSelectedEvent e)
  {
    Debug.Log($"Skill selected: {e.PlayerSkillSO.name}");
    skillSelectionUI.SetActive(false);
    // Update UI or game state based on the selected skill
  }
}