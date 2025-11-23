using System;
using UnityEngine;

[CreateAssetMenu(menuName = "MiniPunBall/Skill/TriggerOnSkillGain")]
public class TriggerOnSkillGain : TriggerSO<SkillGainContext>
{
  public override IDisposable Subscribe(SkillRuntime runtime, Action<SkillGainContext> fire)
  {
    fire(new SkillGainContext(runtime));
    return new DummyDisposable();
  }

  private class DummyDisposable : IDisposable
  {
    public void Dispose() { }
  }
}
