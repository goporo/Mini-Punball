using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
  [SerializeField] private GameObject invincibleVfx;  // flame particle, glow, etc.

  public void SetInvincible(bool invincible)
  {
    if (invincibleVfx != null)
      invincibleVfx.SetActive(invincible);
  }

}
