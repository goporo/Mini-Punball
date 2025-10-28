using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;


public class LevelRuntimeData : Singleton<LevelRuntimeData>
{
  public LevelSO Config { get; }
  public int CurrentWave { get; set; }

}
