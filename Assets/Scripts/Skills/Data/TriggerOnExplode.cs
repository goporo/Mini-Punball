using System;
using UnityEngine;

[CreateAssetMenu(menuName = "MiniPunBall/Skill/TriggerOnExplode")]
public class TriggerOnExplode : TriggerSO<AOEContext>
{
  public override IDisposable Subscribe(SkillRuntime runtime, Action<AOEContext> fire)
  {
    Action<OnBombExplodeEvent> onVoidHit = (OnBombExplodeEvent e) =>
    {
      var context = new AOEContext(e.Context.Targets);
      fire(context);
    };

    EventBus.Subscribe<OnBombExplodeEvent>(onVoidHit);
    return new Unsubscriber(() => EventBus.Unsubscribe(onVoidHit));
  }

}