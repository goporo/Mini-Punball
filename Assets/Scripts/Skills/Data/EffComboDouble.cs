using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "MiniPunBall/Skill/EffComboDouble")]
public class EffComboDouble : EffectSO<ComboCastContext>
{
  [SerializeField] private float chance = 0.25f;
  private float delay = 0.5f;
  public override void Execute(ComboCastContext ctx)
  {
    if (Random.value > chance && ctx.CastOrigin != ECastSource.Combo) return;

    // Start coroutine on player MonoBehaviour
    ctx.Player.StartCoroutine(DelayedExecute(ctx));
  }

  private IEnumerator DelayedExecute(ComboCastContext ctx)
  {
    yield return new WaitForSeconds(delay);
    var newCtx = new EffectCastContext(null, ECastSource.Effect);
    ctx.Effect.Execute(newCtx);
  }
}