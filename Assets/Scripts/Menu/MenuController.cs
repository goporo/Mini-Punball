using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
  [SerializeField] private Button btnLoadLevel1;

  private void Awake()
  {
    btnLoadLevel1.onClick.AddListener(HandleBtnLoadLevelClicked);
  }

  private void HandleBtnLoadLevelClicked()
  {
    GlobalContext.Instance.LoadScene(MainScenes.GamePlayScene);
  }
}