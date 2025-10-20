using System;
using UnityEngine;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
  [SerializeField] Transform playerModel;

  void OnEnable()
  {
    BallPhysics.OnBallReturned += MoveCharacter;
  }
  void OnDisable()
  {
    BallPhysics.OnBallReturned -= MoveCharacter;
  }

  public void MoveCharacter(Vector3 pos)
  {
    float newX = Mathf.Clamp(pos.x, -3f, 3f);
    playerModel.DOMoveX(newX, 0.3f);
  }
}