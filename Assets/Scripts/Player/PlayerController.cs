using System;
using UnityEngine;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
  private float leftBoundary = 0f;
  private float rightBoundary = 5f;
  [SerializeField] Transform playerModel;


  public void MoveCharacter(Vector3 pos)
  {
    float newX = Mathf.Clamp(pos.x, leftBoundary, rightBoundary);
    playerModel.DOMoveX(newX, 0.3f);
  }


}