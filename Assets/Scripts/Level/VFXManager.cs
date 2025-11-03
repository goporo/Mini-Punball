using System;
using System.Collections;
using UnityEngine;

public class VFXManager : MonoBehaviour
{
  private int activeEffects = 0;

  public event Action OnAllEffectsFinished;

  /// <summary>
  /// Call when an effect (missile, particle, animation, etc.) starts
  /// </summary>
  public void RegisterEffect()
  {
    activeEffects++;
  }

  /// <summary>
  /// Call when an effect finishes
  /// </summary>
  public void UnregisterEffect()
  {
    activeEffects--;

    if (activeEffects <= 0)
    {
      activeEffects = 0;
      OnAllEffectsFinished?.Invoke();
    }
  }

  /// <summary>
  /// Check if all effects are done
  /// </summary>
  public bool AllEffectsFinished()
  {
    return activeEffects <= 0;
  }

  /// <summary>
  /// Get current number of active effects (for debugging)
  /// </summary>
  public int GetActiveEffectCount()
  {
    return activeEffects;
  }
}