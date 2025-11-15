using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum LevelResult
{
  Win,
  Lose
}
public class PanelLevelComplete : MonoBehaviour
{
  [SerializeField] private TMP_Text textLevelResult;
  [SerializeField] private Button buttonClose;

  void Awake()
  {
    gameObject.SetActive(false);
    buttonClose.onClick.AddListener(HandleButtonCloseClicked);
  }

  private void HandleButtonCloseClicked()
  {
    GlobalContext.Instance.LoadScene(MainScenes.MenuScene);
  }

  public void Setup(LevelResult result)
  {
    // Setup UI based on win/lose
    if (result == LevelResult.Win)
    {
      textLevelResult.text = "CONGRATULATIONS!";
    }
    else
    {
      textLevelResult.text = "YOU DIED!";
    }
    gameObject.SetActive(true);
  }
}