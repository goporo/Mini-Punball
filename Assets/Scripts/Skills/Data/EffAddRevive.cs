using UnityEngine;

[CreateAssetMenu(menuName = "MiniPunBall/Skill/EffAddRevive")]
public class EffAddRevive : EffectSO
{
  private readonly int count = 1;
  public override void Execute(IEffectContext ctx)
  {
    ctx.Player.HealthComponent.AddReviveCount(count);
  }

}


