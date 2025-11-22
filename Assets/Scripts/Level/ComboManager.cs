using TMPro;
using UnityEngine;

public class ComboManager : MonoBehaviour
{
  private int currentCombo = 0;
  public int CurrentCombo => currentCombo;
  public TMP_Text comboText;
  private float discount = 0f;

  void Awake()
  {
    comboText.gameObject.SetActive(false);
  }

  void OnEnable()
  {
    EventBus.Subscribe<AllBallReturnedEvent>(ResetCombo);
    EventBus.Subscribe<OnComboDiscountAddedEvent>(AddComboDiscount);
  }

  void OnDisable()
  {
    EventBus.Unsubscribe<AllBallReturnedEvent>(ResetCombo);
    EventBus.Unsubscribe<OnComboDiscountAddedEvent>(AddComboDiscount);

  }

  private void AddComboDiscount(OnComboDiscountAddedEvent e)
  {
    discount += e.Discount;
    if (discount > 0.9f)
    {
      discount = 0.9f;
    }
  }

  public int GetDiscountedComboThreshold(int baseThreshold)
  {
    return Mathf.CeilToInt(baseThreshold * (1 - discount));
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
    EventBus.Publish(new OnComboChangedEvent(currentCombo));
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