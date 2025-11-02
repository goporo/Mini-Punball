using TMPro;
using UnityEngine;

public class ComboManager : MonoBehaviour
{
  private int currentCombo = 0;
  public int CurrentCombo => currentCombo;
  public TMP_Text comboText;

  void Awake()
  {
    comboText.gameObject.SetActive(false);
  }

  void OnEnable()
  {
    EventBus.Subscribe<AllBallReturnedEvent>(ResetCombo);
  }

  void OnDisable()
  {
    EventBus.Unsubscribe<AllBallReturnedEvent>(ResetCombo);
  }

  public void ResetCombo(AllBallReturnedEvent e)
  {
    currentCombo = 0;
    UpdateComboUI();
  }

  public void Increment(int amount = 1)
  {
    currentCombo += amount;
    UpdateComboUI();
  }

  private void UpdateComboUI()
  {
    if (currentCombo > 1)
    {
      comboText.gameObject.SetActive(true);
      comboText.text = $"{currentCombo} <br>combo";
    }
    else
    {
      comboText.gameObject.SetActive(false);
    }
  }
}