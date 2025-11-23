using UnityEngine;

[CreateAssetMenu(menuName = "MiniPunBall/Skill/EffBuffHP")]
public class EffBuffHP : EffectSO
{
  [SerializeField] private float multiplier = 1.2f;
  public override void Execute(IEffectContext ctx)
  {
    ctx.Player.ApplyHealthBuff(multiplier);
  }

}


