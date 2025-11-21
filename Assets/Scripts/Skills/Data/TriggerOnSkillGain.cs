using System;
using UnityEngine;

[CreateAssetMenu(menuName = "MiniPunBall/Skill/TriggerOnSkillGain")]
public class TriggerOnSkillGain : TriggerSO<PlayerContext>
{
  public override IDisposable Subscribe(SkillRuntime runtime, Action<PlayerContext> fire)
  {
    fire(new PlayerContext(LevelContext.Instance.Player));
    return new DummyDisposable();
  }

  private class DummyDisposable : IDisposable
  {
    public void Dispose() { }
  }
}
