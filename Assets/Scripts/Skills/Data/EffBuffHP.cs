using UnityEngine;

[CreateAssetMenu(menuName = "MiniPunBall/Skill/EffBuffHP")]
public class EffBuffHP : EffectSO
{
  private readonly float buffMultiplier = 1.2f;
  public override void Execute(IEffectContext ctx)
  {
    LevelContext.Instance.Player.ApplyHealthBuff(buffMultiplier);
  }

}


