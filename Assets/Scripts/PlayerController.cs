using System;
using UnityEngine;
using DG.Tweening;

public class PlayerController : Singleton<PlayerController>
{
  [SerializeField] Transform playerModel;
  void OnEnable()
  {
    BallBehaviour.OnBallReturned += HandleBallReturn;
  }

  void OnDisable()
  {
    BallBehaviour.OnBallReturned -= HandleBallReturn;
  }

  public void HandleBallReturn(Vector3 ballPos)
  {
    if (ballPos == Vector3.zero) return;
    MoveCharacter(ballPos);
  }

  void MoveCharacter(Vector3 pos)
  {
    playerModel.DOMoveX(pos.x, 0.3f);
  }
}