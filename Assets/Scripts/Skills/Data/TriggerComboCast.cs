using System;
using UnityEngine;

[CreateAssetMenu(menuName = "MiniPunBall/Skill/TriggerComboCast")]
public class TriggerComboCast : TriggerSO<EffectCastContext>
{

  public override IDisposable Subscribe(SkillRuntime runtime, Action<EffectCastContext> fire)
  {
    Action<OnComboCastEvent> onComboCast = (OnComboCastEvent e) =>
    {
      var context = new EffectCastContext(e.Effect);
      fire(context);
    };

    EventBus.Subscribe<OnComboCastEvent>(onComboCast);

    return new Unsubscriber(() =>
    {
      EventBus.Unsubscribe(onComboCast);
    });
  }

}