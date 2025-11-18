using System;
using System.Collections;
using UnityEngine;


public class LevelContext : Singleton<LevelContext>
{
  public PlayerRunStats Player { get; private set; }
  public LevelController LevelController { get; private set; }
  public WaveController WaveController { get; private set; }
  public SkillManager SkillManager { get; private set; }
  public ComboManager ComboManager { get; private set; }
  public PickupManager PickupManager { get; private set; }
  public BoardState BoardState { get; private set; }
  public VFXManager VFXManager { get; private set; }
  public UIPool UIPool { get; private set; }

  protected override void Awake()
  {
    base.Awake();

    Player = FindObjectOfType<PlayerRunStats>();
    LevelController = FindObjectOfType<LevelController>();
    WaveController = FindObjectOfType<WaveController>();
    SkillManager = FindObjectOfType<SkillManager>();
    ComboManager = FindObjectOfType<ComboManager>();
    PickupManager = FindObjectOfType<PickupManager>();
    BoardState = FindObjectOfType<BoardState>();
    VFXManager = FindObjectOfType<VFXManager>();
    UIPool = FindObjectOfType<UIPool>();
  }

  public IEnumerator CoroutineCleanUp()
  {
    Player.Balls.ForceBallsReturn();
    // BoardState.ForceClearBoard();
    yield return PickupManager.ForceClearAllCollects();
    // ForceStopAllCoroutines();
  }

  private void ForceStopAllCoroutines()
  {
    WaveController?.StopAllCoroutines();
    SkillManager?.StopAllCoroutines();
    ComboManager?.StopAllCoroutines();
    PickupManager?.StopAllCoroutines();
    BoardState?.StopAllCoroutines();
    VFXManager?.StopAllCoroutines();
    UIPool?.StopAllCoroutines();
    // Add more as needed
  }
}

