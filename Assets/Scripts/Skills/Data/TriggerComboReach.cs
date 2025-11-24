using System;
using UnityEngine;

[CreateAssetMenu(menuName = "MiniPunBall/Skill/TriggerComboReach")]
public class TriggerComboReach : TriggerSO<EffectCastContext>
{
  [SerializeField] private int comboThreshold = 40;

  public override IDisposable Subscribe(SkillRuntime runtime, Action<EffectCastContext> fire)
  {
    Action<OnComboChangedEvent> onComboChange = (OnComboChangedEvent e) =>
    {
      var currentThreshold = LevelContext.Instance.ComboManager.GetDiscountedComboThreshold(comboThreshold);
      if (currentThreshold <= 0 || e.CurrentCombo % currentThreshold != 0) return;
      var context = new EffectCastContext(null, ECastSource.Combo);
      fire(context);

    };

    EventBus.Subscribe<OnComboChangedEvent>(onComboChange);

    return new Unsubscriber(() =>
    {
      EventBus.Unsubscribe(onComboChange);
    });
  }

}