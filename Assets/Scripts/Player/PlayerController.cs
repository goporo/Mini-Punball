using System;
using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
  private float leftBoundary = 0f;
  private float rightBoundary = 5f;
  [SerializeField] Transform playerModel;

  void OnEnable()
  {
    BallManager.OnAllBallsReturned += HandleAllBallsReturned;
  }

  void OnDisable()
  {
    BallManager.OnAllBallsReturned -= HandleAllBallsReturned;
  }
  void HandleAllBallsReturned(List<BallBase> balls)
  {
    Debug.Log("Total Balls Returned: " + balls.Count);
    MoveCharacter(balls[0].transform.position);
  }
  public void MoveCharacter(Vector3 pos)
  {
    float newX = Mathf.Clamp(pos.x, leftBoundary, rightBoundary);
    playerModel.DOMoveX(newX, 0.3f);
  }


}