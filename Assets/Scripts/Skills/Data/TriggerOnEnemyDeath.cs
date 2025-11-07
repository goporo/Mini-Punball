using System;
using UnityEngine;

[CreateAssetMenu(menuName = "MiniPunBall/Skill/TriggerOnEnemyDeath", order = 0)]
public class TriggerOnEnemyDeath : TriggerSO<BallHitContext>
{
  public override IDisposable Subscribe(SkillRuntime runtime, Action<BallHitContext> fire)
  {
    Action<EnemyDeathEvent> onEnemyDeath = (EnemyDeathEvent e) =>
    {
      var context = new BallHitContext(e.Context.Enemy, e.Context.Damage, null);
      fire(context);
    };

    EventBus.Subscribe<EnemyDeathEvent>(onEnemyDeath);
    return new Unsubscriber(() => EventBus.Unsubscribe(onEnemyDeath));
  }

}