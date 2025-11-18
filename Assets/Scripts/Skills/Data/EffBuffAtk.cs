using UnityEngine;

[CreateAssetMenu(menuName = "MiniPunBall/Skill/EffBuffAtk", order = 0)]
public class EffBuffAtk : EffectSO
{
  private readonly float buffMultiplier = 1.1f;
  public override void Execute(IEffectContext ctx)
  {
    LevelContext.Instance.Player.ApplyAttackBuff(buffMultiplier);
  }

}


