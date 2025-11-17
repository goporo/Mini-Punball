using System.Collections;
using DG.Tweening;
using UnityEngine;

public abstract class EnemySkillBehavior : ScriptableObject
{
  public abstract IEnumerator UseSkill(Enemy enemy, BoardState board);
  public virtual IEnumerator AttackAndDie(BoardObject boardObject, PlayerDamageContext ctx)
  {
    // Simple jump forward and shake animation
    Transform t = boardObject.transform;
    Vector3 originalPos = t.position;
    Vector3 attackPos = originalPos - t.forward * 0.7f;

    Sequence seq = DOTween.Sequence();
    seq.Append(t.DOJump(attackPos, 0.5f, 1, 0.18f).SetEase(Ease.OutQuad)); // Jump forward
    seq.Append(t.DOShakePosition(0.12f, 0.18f, 8, 90, false, true)); // Shake on impact

    yield return seq.WaitForCompletion();

    if (ctx.FinalDamage > 0)
    {
      CombatResolver.Instance.PlayerTakeDamage(ctx);

    }

    boardObject.HandleOnSacrifice();
    yield break;
  }

}
