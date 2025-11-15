using UnityEngine;

public class RegisterableEffect : MonoBehaviour, IRegisterableEffect
{
  // Use for long living VFX that need to be tracked by VFXManager
  public virtual void RegisterEffect()
  {
    // Register this effect when it spawns
    LevelContext.Instance.VFXManager.RegisterEffect();
  }

  public virtual void UnregisterEffect()
  {
    // Unregister this effect from the VFXManager
    LevelContext.Instance.VFXManager.UnregisterEffect();
  }

}