using UnityEngine;

[CreateAssetMenu(menuName = "MiniPunBall/Skill/EffComboDiscount")]
public class EffComboDiscount : EffectSO
{
  [SerializeField] private float discountValue = 0.1f;
  public override void Execute(IEffectContext ctx)
  {
    EventBus.Publish(new OnComboDiscountAddedEvent(discountValue));

  }
}


