using System;
using UnityEngine;

[CreateAssetMenu(menuName = "MiniPunBall/Skill/TriggerOnSkillGain")]
public class TriggerOnSkillGain : TriggerSO
{
  public override IDisposable Subscribe(SkillRuntime runtime, Action<IEffectContext> fire)
  {
    fire(new EffectContext(null));
    Debug.Log("Fired on skill gain!");
    return new DummyDisposable();
  }

  private class DummyDisposable : IDisposable
  {
    public void Dispose() { }
  }
}
