using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "MiniPunBall/Skill/EffComboDouble")]
public class EffComboDouble : EffectSO<EffectCastContext>
{
  private float chance = 0.25f;
  private float delay = 0.5f;
  public override void Execute(EffectCastContext ctx)
  {
    if (Random.value > chance && !ctx.IsComboCast) return;

    // Start coroutine on player MonoBehaviour
    ctx.Player.StartCoroutine(DelayedExecute(ctx));
  }

  private IEnumerator DelayedExecute(EffectCastContext ctx)
  {
    yield return new WaitForSeconds(delay);
    var newCtx = new EffectCastContext(ctx.Effect, false);
    ctx.Effect.Execute(newCtx);
  }
}