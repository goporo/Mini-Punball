using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(CanvasGroup))]
public class PanelBossComing : CanvasGroupUIBase
{

  protected override void Awake()
  {
    base.Awake();
    Hide();
  }

  private void OnEnable()
  {
    EventBus.Subscribe<OnBossWaveApproachingEvent>(HandleOnBossWaveApproaching);
  }

  void OnDisable()
  {
    EventBus.Unsubscribe<OnBossWaveApproachingEvent>(HandleOnBossWaveApproaching);
  }

  private void HandleOnBossWaveApproaching(OnBossWaveApproachingEvent e)
  {
    BlinkShowAndHide();
  }

  public void BlinkShowAndHide()
  {
    Show();
    cg.alpha = 0.5f;
    BlinkSequence();
  }

  private void BlinkSequence()
  {
    const float fadeDuration = 0.3f;
    var seq = DG.Tweening.DOTween.Sequence();
    for (int i = 0; i < 3; i++)
    {
      seq.Append(cg.DOFade(1f, fadeDuration));
      seq.Append(cg.DOFade(0f, fadeDuration));
    }
    seq.OnComplete(() => Hide());
  }
}