using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class CGBase : MonoBehaviour
{
  protected CanvasGroup cg;

  protected virtual void Awake()
  {
    cg = GetComponent<CanvasGroup>();
  }

  public virtual void Show()
  {
    cg.alpha = 1;
  }

  public virtual void Hide()
  {
    cg.alpha = 0;
  }
}