using UnityEngine;

[CreateAssetMenu(menuName = "MiniPunBall/Skill/EffAddMasterFire")]
public class EffAddMasterFire : EffectSO
{
  public override void Execute(IEffectContext ctx)
  {
    CombatResolver.Instance.InjectGlobalModifier(
      new FireMasterMod()
    );
    StatusExecutor.Instance.AddModifier(
        new FireMasterStatusMod()
      );
  }

}

