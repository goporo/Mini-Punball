using System.Collections;
using UnityEngine;
using DG.Tweening;

[CreateAssetMenu(menuName = "MiniPunBall/Enemy/ESkillNone")]
public class ESkillNone : EnemySkillBehavior
{
  public override IEnumerator UseSkill(Enemy enemy, BoardState board)
  {
    yield break;
  }

  public override IEnumerator AttackAndDie(BoardObject boardObject, DamageContext damageContext)
  {
    boardObject.HandleOnSacrifice();
    yield break;
  }

}