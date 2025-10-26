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
    BallManager.OnAllBallsReturned += HandleAllBallsReturned;
    PlayerShooter.OnBallFired += HandleBallFired;
  }

  void OnDisable()
  {
    BallManager.OnAllBallsReturned -= HandleAllBallsReturned;
    PlayerShooter.OnBallFired -= HandleBallFired;
  }
  void HandleAllBallsReturned(List<BallBase> balls)
  {
    MoveCharacter(balls[0].transform.position);
  }
  void HandleBallFired(BallBase ball)
  {
    playerUI.OnBallCountChanged(ballManager.RemainingBalls);
    ballManager.RegisterBall(ball);
  }
  public void MoveCharacter(Vector3 pos)
  {
    float newX = Mathf.Clamp(pos.x, leftBoundary, rightBoundary);
    transform.DOMoveX(newX, 0.3f);
  }


}