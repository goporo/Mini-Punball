using System.Collections;
using UnityEngine;
using DG.Tweening;

[CreateAssetMenu(menuName = "MiniPunBall/MoveBehavior/MoveUpOne")]
public class MoveUpOne : MoveBehavior
{
  public override Vector2Int GetTargetCell(BoardObject boardObject, BoardState board)
      => boardObject.CurrentCell - new Vector2Int(0, 1);


}