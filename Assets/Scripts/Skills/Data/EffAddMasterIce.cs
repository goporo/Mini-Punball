using UnityEngine;

[CreateAssetMenu(menuName = "MiniPunBall/Skill/EffAddMasterIce")]
public class EffAddMasterIce : EffectSO
{
  public override void Execute(IEffectContext ctx)
  {
    CombatResolver.Instance.InjectGlobalModifier(
      new IceMasterMod()
    );
    StatusExecutor.Instance.AddModifier(
        new IceMasterStatusMod()
      );
  }

}

