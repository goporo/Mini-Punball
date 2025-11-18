using UnityEngine;
using DG.Tweening;
using System.Collections;
using TMPro;

public class PanelNewSkillInfo : MonoBehaviour
{
  [SerializeField] private TMP_Text titleText;
  [SerializeField] private TMP_Text descText;

  private RectTransform rectTransform;
  private Vector2 hiddenPos;
  private Vector2 shownPos;
  private float animDuration = 0.3f;
  private Tween currentTween;
  private Coroutine hideCoroutine;

  private void Awake()
  {
    rectTransform = GetComponent<RectTransform>();
    float height = rectTransform.rect.height;
    shownPos = rectTransform.anchoredPosition;
    hiddenPos = shownPos + new Vector2(0, height);
    rectTransform.anchoredPosition = hiddenPos;
    gameObject.SetActive(false);
  }

  public void Setup(string title, string desc)
  {
    if (titleText != null) titleText.text = title;
    if (descText != null) descText.text = desc;
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