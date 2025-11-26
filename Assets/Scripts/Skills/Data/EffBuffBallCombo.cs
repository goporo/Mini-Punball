using UnityEngine;

[CreateAssetMenu(menuName = "MiniPunBall/Skill/EffBuffBallCombo")]
public class EffBuffBallCombo : EffectSO
{
  [SerializeField] private BallBuffTarget target;
  public override void Execute(IEffectContext ctx)
  {
    IComboModifier mod = null;
    switch (target)
    {
      case BallBuffTarget.Special:
        mod = new SpecialBallModifier(5);
        break;
    }
    LevelContext.Instance.ComboManager.InjectComboModifier(
      mod
    );

  }

}

