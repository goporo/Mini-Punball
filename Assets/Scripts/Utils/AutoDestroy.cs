using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class AutoDestroy : MonoBehaviour
{
  private ParticleSystem ps;

  void Awake() => ps = GetComponent<ParticleSystem>();

  void Update()
  {
    if (ps && !ps.IsAlive()) Destroy(gameObject);
  }
}
