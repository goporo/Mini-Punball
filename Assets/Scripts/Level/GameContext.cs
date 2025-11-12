using System;
using UnityEngine;
using UnityEngine.VFX;


public class GameContext : Singleton<GameContext>
{

  public PlayerRunStats Player { get; private set; }
  public LevelController LevelController { get; private set; }
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
    SkillManager = FindObjectOfType<SkillManager>();
    ComboManager = FindObjectOfType<ComboManager>();
    PickupManager = FindObjectOfType<PickupManager>();
    BoardState = FindObjectOfType<BoardState>();
    VFXManager = FindObjectOfType<VFXManager>();
    UIPool = FindObjectOfType<UIPool>();
  }
}

