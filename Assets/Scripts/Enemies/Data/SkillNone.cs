using System.Collections;
using UnityEngine;
using DG.Tweening;

[CreateAssetMenu(menuName = "MiniPunBall/Enemy/SkillNone")]
public class SkillNone : EnemySkillBehavior
{
  public override IEnumerator UseSkill(BoardObject boardObject, BoardState board)
  {
    yield break;
  }


}