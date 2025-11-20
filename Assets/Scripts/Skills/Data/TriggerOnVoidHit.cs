using System;
using UnityEngine;

[CreateAssetMenu(menuName = "MiniPunBall/Skill/TriggerOnVoidHit", order = 0)]
public class TriggerOnVoidHit : TriggerSO<EffectContext>
{
  public override IDisposable Subscribe(SkillRuntime runtime, Action<EffectContext> fire)
  {
    Action<OnVoidHitEvent> onVoidHit = (OnVoidHitEvent e) =>
    {
      var context = new EffectContext(e.Context.Enemy);
      fire(context);
    };

    EventBus.Subscribe<OnVoidHitEvent>(onVoidHit);
    return new Unsubscriber(() => EventBus.Unsubscribe(onVoidHit));
  }

}