using System;
using UnityEngine;

[CreateAssetMenu(menuName = "MiniPunBall/Skill/TriggerEveryNWave")]
public class TriggerEveryNWave : TriggerSO<PlayerContext>
{
  [SerializeField] int wavesRequire;
  private int wavePassed;
  private Action<PlayerContext> fireAction;
  private Action<OnWaveStartEvent> waveStartHandler;

  public override IDisposable Subscribe(SkillRuntime runtime, Action<PlayerContext> fire)
  {
    // Execute effect immediately upon subscription
    fire(new PlayerContext(LevelContext.Instance.Player));

    // And Execute effect every N waves
    wavePassed = 0;
    fireAction = fire;
    waveStartHandler = HandleWaveStart;
    EventBus.Subscribe<OnWaveStartEvent>(waveStartHandler);
    return new Unsubscriber(UnsubscribeWaveStart);
  }

  private void HandleWaveStart(OnWaveStartEvent e)
  {
    wavePassed++;
    if (wavePassed % wavesRequire == 0)
    {
      PlayerContext context = new PlayerContext(LevelContext.Instance.Player);
      fireAction?.Invoke(context);
    }
  }

  private void UnsubscribeWaveStart()
  {
    EventBus.Unsubscribe(waveStartHandler);
  }
}
