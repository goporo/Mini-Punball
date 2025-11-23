using UnityEngine;

[CreateAssetMenu(menuName = "MiniPunBall/Skill/EffBuffHealPickup")]
public class EffBuffHealPickup : EffectSO
{
  [SerializeField] private float multiplier = 1.5f;
  public override void Execute(IEffectContext ctx)
  {
    ctx.Player.ApplyHealPickupBuff(multiplier);
  }

}


