using System;
using System.Collections.Generic;

[Serializable]
public class SkillRuntime : IDisposable
{
  public PlayerSkillSO skill;
  public PlayerRunStats player => LevelContext.Instance.Player;
  public List<IDisposable> subscriptions = new();
  public int stackCount = 1; // Track skill stacks

  public SkillRuntime(PlayerSkillSO skill)
  {
    this.skill = skill;
  }

  public void Dispose()
  {
    foreach (var d in subscriptions)
      d?.Dispose();
    subscriptions.Clear();
  }
}