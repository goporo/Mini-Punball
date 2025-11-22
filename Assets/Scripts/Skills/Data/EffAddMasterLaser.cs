using UnityEngine;

[CreateAssetMenu(menuName = "MiniPunBall/Skill/EffAddMasterLaser")]
public class EffAddMasterLaser : EffectSO
{
  public override void Execute(IEffectContext ctx)
  {
    CombatResolver.Instance.InjectGlobalModifier(
      new LaserMasterMod()
    );
    EffectExecutor.Instance.AddModifier(
      new LaserMasterEffectMod()
    );
  }

}

