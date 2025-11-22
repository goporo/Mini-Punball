using UnityEngine;

[CreateAssetMenu(menuName = "MiniPunBall/Skill/EffComboDiscount")]
public class EffComboDiscount : EffectSO<PlayerContext>
{
  [SerializeField] private float discountValue = 0.1f;
  public override void Execute(PlayerContext ctx)
  {
    EventBus.Publish(new OnComboDiscountAddedEvent(discountValue));

  }
}


