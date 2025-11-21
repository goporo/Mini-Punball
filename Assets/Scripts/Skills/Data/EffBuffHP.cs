using UnityEngine;

[CreateAssetMenu(menuName = "MiniPunBall/Skill/EffBuffHP")]
public class EffBuffHP : EffectSO
{
  [SerializeField] private float buffPercent = 20;
  public override void Execute(IEffectContext ctx)
  {
    ctx.Player.ApplyHealthBuff(1 + buffPercent / 100f);
  }

}


