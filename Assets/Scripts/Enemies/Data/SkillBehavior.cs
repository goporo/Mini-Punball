using System.Collections;
using DG.Tweening;
using UnityEngine;

public abstract class SkillBehavior : ScriptableObject
{
  public abstract IEnumerator AttackSKill(BoardObject boardObject, BoardState board);
  public virtual IEnumerator AttackAndDie(BoardObject boardObject, BoardState board)
  {
    yield break;
  }
}
