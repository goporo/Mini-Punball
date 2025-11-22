using UnityEngine;

[CreateAssetMenu(menuName = "MiniPunBall/Skill/EffAddMasterMissile")]
public class EffAddMasterMissile : EffectSO
{
  public override void Execute(IEffectContext ctx)
  {
    CombatResolver.Instance.InjectGlobalModifier(
      new MissileMasterMod()
    );
    EffectExecutor.Instance.AddModifier(
      new MissileMasterEffectMod()
    );
  }

}

