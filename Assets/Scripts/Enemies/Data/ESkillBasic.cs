using System.Collections;
using UnityEngine;
using DG.Tweening;

[CreateAssetMenu(menuName = "MiniPunBall/Enemy/ESkillBasic")]
public class ESkillBasic : EnemySkillBehavior
{
  public override IEnumerator UseSkill(Enemy enemy, BoardState board)
  {
    yield break;
  }


}