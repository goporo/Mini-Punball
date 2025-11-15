using System;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(HealthComponent))]
public class PlayerController : MonoBehaviour
{
  private float leftBoundary = 0f;
  private float rightBoundary = 5f;
  [SerializeField] private PlayerUI playerUI;
  [SerializeField] private BallManager ballManager;
  private HealthComponent healthComponent;


  void Awake()
  {
    healthComponent = GetComponent<HealthComponent>();
    int maxHealth = GlobalContext.Instance.CharacterSO.BaseHealth;
    playerUI.Init(maxHealth);
    healthComponent.Init(maxHealth);
  }
  void OnEnable()
  {
    EventBus.Subscribe<AllBallReturnedEvent>(HandleAllBallsReturned);
    EventBus.Subscribe<PlayerCanShootEvent>(HandlePlayerCanShoot);
    healthComponent.OnDied += HandleDeath;
  }
  void OnDisable()
  {
    EventBus.Unsubscribe<AllBallReturnedEvent>(HandleAllBallsReturned);
    EventBus.Unsubscribe<PlayerCanShootEvent>(HandlePlayerCanShoot);
    healthComponent.OnDied -= HandleDeath;
  }
  private void HandlePlayerCanShoot(PlayerCanShootEvent e)
  {
    playerUI.UpdateBallCount(ballManager.playerBalls.Count);
    if (e.CanShoot) playerUI.EnableTextBall(true);
  }

  void HandleAllBallsReturned(AllBallReturnedEvent e)
  {
    if (e.ReturnedBalls.Count > 0)
      MoveCharacter(e.ReturnedBalls[0].transform.position);
  }

  public void MoveCharacter(Vector3 pos)
  {
    float newX = Mathf.Clamp(pos.x, leftBoundary, rightBoundary);
    transform.DOMoveX(newX, 0.3f);
  }

  public void HandleDeath()
  {
    EventBus.Publish(new OnPlayerDiedEvent());
  }

}