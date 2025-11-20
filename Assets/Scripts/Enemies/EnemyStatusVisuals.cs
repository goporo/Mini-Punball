using UnityEngine;

public class EnemyStatusVisuals : MonoBehaviour
{
  [Header("Burn")]
  [SerializeField] private GameObject burnVfx;  // flame particle, glow, etc.

  [Header("Frozen")]
  [SerializeField] private GameObject frozenVfx;

  public void SetBurning(bool burning)
  {
    if (burnVfx != null)
      burnVfx.SetActive(burning);
  }

  public void SetFrozen(bool frozen)
  {
    if (frozenVfx != null)
      frozenVfx.SetActive(frozen);
  }
}
