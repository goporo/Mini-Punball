using System.Collections;
using UnityEngine;
using DG.Tweening;

[CreateAssetMenu(menuName = "MiniPunBall/SkillBehavior/SkillNone")]
public class SkillNone : SkillBehavior
{
  public override IEnumerator UseSkill(BoardObject boardObject, BoardState board)
  {
    yield break;
  }


}