using UnityEngine;

[CreateAssetMenu(menuName = "MiniPunBall/Skill/EffBuffBall")]
public class EffBuffBall : EffectSO
{
  [SerializeField] BallBuffTarget target;
  [SerializeField] private float multiplier = 1.2f;
  public override void Execute(IEffectContext ctx)
  {
    LevelContext.Instance.Player.Balls.ApplyBallBuff(
        new BallBuff(multiplier, target)
    );
  }

}


