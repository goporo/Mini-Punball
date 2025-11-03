/// <summary>
/// Interface for effects that need to register/unregister with the VFXManager
/// </summary>
public interface IRegisterableEffect
{
  /// <summary>
  /// Register this effect with the VFXManager
  /// </summary>
  void RegisterEffect();

  /// <summary>
  /// Unregister this effect from the VFXManager
  /// </summary>
  void UnregisterEffect();
}
