using System;
using UnityEngine;

[CreateAssetMenu(menuName = "MiniPunBall/Skill/TriggerOnVoidHit")]
public class TriggerOnVoidHit : TriggerSO<EffectCastContext>
{
  public override IDisposable Subscribe(SkillRuntime runtime, Action<EffectCastContext> fire)
  {
    Action<OnVoidHitEvent> onVoidHit = (OnVoidHitEvent e) =>
    {
      var context = new EffectCastContext(e.Context.Enemy, ECastSource.Enemy);
      fire(context);
    };

    EventBus.Subscribe<OnVoidHitEvent>(onVoidHit);
    return new Unsubscriber(() => EventBus.Unsubscribe(onVoidHit));
  }

}