using UnityEngine;

[CreateAssetMenu(menuName = "MiniPunBall/Enemy/StatusNone")]
public class StatusNone : StatusEffectSO
{
  protected override IStatusEffect CreateRuntimeInstance()
  {
    return null;
  }

}
