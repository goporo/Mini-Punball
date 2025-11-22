using UnityEngine;

[CreateAssetMenu(menuName = "MiniPunBall/Skill/EffAddMasterLightning")]
public class EffAddMasterLightning : EffectSO
{
  public override void Execute(IEffectContext ctx)
  {
    CombatResolver.Instance.InjectGlobalModifier(
      new LightningMasterMod()
    );
    EffectExecutor.Instance.AddModifier(
      new LightningMasterEffectMod()
    );
  }

}

