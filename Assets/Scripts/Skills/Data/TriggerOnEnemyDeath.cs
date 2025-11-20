using System;
using UnityEngine;

[CreateAssetMenu(menuName = "MiniPunBall/Skill/TriggerOnEnemyDeath")]
public class TriggerOnEnemyDeath : TriggerSO<EffectContext>
{
  public override IDisposable Subscribe(SkillRuntime runtime, Action<EffectContext> fire)
  {
    Action<EnemyDeathEvent> onEnemyDeath = (EnemyDeathEvent e) =>
    {
      var context = new EffectContext(e.Context.Enemy);
      fire(context);
    };

    EventBus.Subscribe<EnemyDeathEvent>(onEnemyDeath);
    return new Unsubscriber(() => EventBus.Unsubscribe(onEnemyDeath));
  }

}