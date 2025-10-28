using System;
using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
  private float leftBoundary = 0f;
  private float rightBoundary = 5f;
  [SerializeField] PlayerUI playerUI;
  [SerializeField] BallManager ballManager;


  void Awake()
  {
    int maxHealth = GameManager.Instance.CharacterSO.BaseHealth;
    playerUI.Init(maxHealth);
  }
  void OnEnable()
  {
    EventBus.Subscribe<AllBallReturnedEvent>(HandleAllBallsReturned);
    EventBus.Subscribe<PlayerCanShootEvent>(HandlePlayerCanShoot);
  }
  void OnDisable()
  {
    EventBus.Unsubscribe<AllBallReturnedEvent>(HandleAllBallsReturned);
    EventBus.Unsubscribe<PlayerCanShootEvent>(HandlePlayerCanShoot);
  }
  private void HandlePlayerCanShoot(PlayerCanShootEvent e)
  {
    playerUI.UpdateBallCount(ballManager.playerBalls.Count);
    playerUI.EnableTextBall(true);
  }

  void HandleAllBallsReturned(AllBallReturnedEvent e)
  {
    MoveCharacter(e.ReturnedBalls[0].transform.position);
  }

  public void MoveCharacter(Vector3 pos)
  {
    float newX = Mathf.Clamp(pos.x, leftBoundary, rightBoundary);
    transform.DOMoveX(newX, 0.3f);
  }


}