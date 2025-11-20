using UnityEngine;

[CreateAssetMenu(menuName = "MiniPunBall/Skill/EffApplyStatus")]
public class EffApplyStatus : EffectSO<AOEContext>
{
  [SerializeField] private StatusEffectSO statusEffect;
  public override void Execute(AOEContext ctx)
  {
    for (int i = 0; i < ctx.Targets.Count; i++)
    {
      statusEffect.ApplyTo(ctx.Targets[i]);
    }
  }
}


