using System;
using UnityEngine;

[CreateAssetMenu(menuName = "MiniPunBall/Skill/TriggerComboCast")]
public class TriggerComboCast : TriggerSO<ComboCastContext>
{

  public override IDisposable Subscribe(SkillRuntime runtime, Action<ComboCastContext> fire)
  {
    Action<OnComboCastEvent> onComboCast = (OnComboCastEvent e) =>
    {
      var context = new ComboCastContext(e.Effect);
      fire(context);
    };

    EventBus.Subscribe<OnComboCastEvent>(onComboCast);

    return new Unsubscriber(() =>
    {
      EventBus.Unsubscribe(onComboCast);
    });
  }

}