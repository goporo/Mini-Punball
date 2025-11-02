using System;
using UnityEngine;

[CreateAssetMenu(menuName = "MiniPunBall/Skill/TriggerOnEnemyDeath", order = 0)]
public class TriggerOnEnemyDeath : TriggerSO<BallHitContext>
{
  public override IDisposable Subscribe(SkillRuntime runtime, Action<BallHitContext> fire)
  {
    Action<EnemyDeathEvent> onEnemyDeath = e =>
    {
      var context = new BallHitContext(e.Context.Enemy, e.Context.Damage, null);
      fire(context);
      Debug.Log("[TriggerOnEnemyDeath] Enemy died, trigger fired");
    };

    EventBus.Subscribe(onEnemyDeath);
    return new Unsubscriber(() => EventBus.Unsubscribe(onEnemyDeath));
  }
}