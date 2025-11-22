using UnityEngine;

[CreateAssetMenu(menuName = "MiniPunBall/Skill/EffBuffHealPickup")]
public class EffBuffHealPickup : EffectSO
{
  [SerializeField] private float buffPercent = 50;
  public override void Execute(IEffectContext ctx)
  {
    ctx.Player.ApplyHealPickupBuff(1 + buffPercent / 100f);
  }

}


