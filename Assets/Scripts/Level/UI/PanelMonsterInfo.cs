using UnityEngine;
using DG.Tweening;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class PanelMonsterInfo : MonoBehaviour
{
  [SerializeField] private TMP_Text titleText;
  [SerializeField] private TMP_Text descText;
  [SerializeField] private Image iconImage;

  private RectTransform rectTransform;
  private Vector2 hiddenPos;
  private Vector2 shownPos;
  private float animDuration = 0.3f;
  private Tween currentTween;
  private Coroutine hideCoroutine;

  private void Awake()
  {
    rectTransform = GetComponent<RectTransform>();
    float width = rectTransform.rect.width;
    shownPos = rectTransform.anchoredPosition;
    hiddenPos = shownPos + new Vector2(width, 0);
    rectTransform.anchoredPosition = hiddenPos;
    gameObject.SetActive(false);
  }

  public void Setup(string title, string desc, Sprite icon)
  {
    if (titleText != null) titleText.text = title;
    if (descText != null) descText.text = desc;
    if (iconImage != null) iconImage.sprite = icon;
  }

  public void Show()
  {
    if (currentTween != null && currentTween.IsActive()) currentTween.Kill();
    gameObject.SetActive(true);
    rectTransform.anchoredPosition = hiddenPos;
    currentTween = rectTransform.DOAnchorPos(shownPos, animDuration)
      .SetUpdate(true)
      .OnComplete(() => currentTween = null);
  }

  public void Hide()
  {
    if (currentTween != null && currentTween.IsActive()) currentTween.Kill();
    currentTween = rectTransform.DOAnchorPos(hiddenPos, animDuration)
      .SetUpdate(true)
      .OnComplete(() =>
      {
        gameObject.SetActive(false);
        currentTween = null;
      });
  }

  public void ShowFor(float duration)
  {
    if (hideCoroutine != null)
    {
      StopCoroutine(hideCoroutine);
      hideCoroutine = null;
    }
    Show();
    hideCoroutine = StartCoroutine(HideAfterDelay(duration));
  }

  private IEnumerator HideAfterDelay(float delay)
  {
    yield return new WaitForSeconds(delay);
    Hide();
    hideCoroutine = null;
  }
}