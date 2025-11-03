using UnityEngine;

public class RegisterableEffect : MonoBehaviour, IRegisterableEffect
{
  public virtual void RegisterEffect()
  {
    // Register this effect when it spawns
    GameContext.Instance.VFXManager.RegisterEffect();
  }

  public virtual void UnregisterEffect()
  {
    // Unregister this effect from the VFXManager
    GameContext.Instance.VFXManager.UnregisterEffect();
  }

}