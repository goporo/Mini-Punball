using System;
using UnityEngine;

[CreateAssetMenu(menuName = "MiniPunBall/Skill/TriggerOnEnemyDeath")]
public class TriggerOnEnemyDeath : TriggerSO<EffectCastContext>
{
  public override IDisposable Subscribe(SkillRuntime runtime, Action<EffectCastContext> fire)
  {
    Action<EnemyDeathEvent> onEnemyDeath = (EnemyDeathEvent e) =>
    {
      var context = new EffectCastContext(null, e.Context.Enemy, ECastOrigin.Enemy);
      fire(context);
    };

    EventBus.Subscribe<EnemyDeathEvent>(onEnemyDeath);
    return new Unsubscriber(() => EventBus.Unsubscribe(onEnemyDeath));
  }

}