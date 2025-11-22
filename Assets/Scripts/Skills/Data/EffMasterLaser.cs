using UnityEngine;

[CreateAssetMenu(menuName = "MiniPunBall/Skill/EffMasterLaser")]
public class EffMasterLaser : EffectSO
{
  public override void Execute(IEffectContext ctx)
  {
    CombatResolver.Instance.InjectGlobalModifier(
      new LaserMasterMod()
    );
  }

}


