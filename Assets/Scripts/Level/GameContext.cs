using System;
using UnityEngine;


public class GameContext : Singleton<GameContext>
{

  public PlayerRunStats Player { get; private set; }
  public SkillManager SkillManager { get; private set; }
  public ComboManager ComboManager { get; private set; }
  public BoardState BoardState { get; private set; }

  protected override void Awake()
  {
    base.Awake();

    Player = FindObjectOfType<PlayerRunStats>();
    SkillManager = FindObjectOfType<SkillManager>();
    ComboManager = FindObjectOfType<ComboManager>();
    BoardState = FindObjectOfType<BoardState>();
  }
}

