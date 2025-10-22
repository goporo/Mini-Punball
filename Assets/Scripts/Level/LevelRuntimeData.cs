using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;


public class LevelRuntimeData : Singleton<LevelRuntimeData>
{
  public LevelSO Config { get; }
  public int CurrentWave { get; set; }
  public LevelState State { get; set; }
  public bool CanShoot { get; set; } = false;

  // public LevelRuntimeData(LevelSO config)
  // {
  //   Config = config;
  //   CurrentWave = 0;
  //   State = LevelState.None;
  // }

  public bool HasNextWave => CurrentWave < Config.totalWaves;
}
