using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class CanvasGroupUIBase : MonoBehaviour
{
  protected CanvasGroup cg;

  protected virtual void Awake()
  {
    cg = GetComponent<CanvasGroup>();
  }

  public virtual void Show()
  {
    cg.alpha = 1;
    cg.interactable = true;
    cg.blocksRaycasts = true;
  }

  public virtual void Hide()
  {
    cg.alpha = 0;
    cg.interactable = false;
    cg.blocksRaycasts = false;
  }
}