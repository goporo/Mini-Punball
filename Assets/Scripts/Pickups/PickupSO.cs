using UnityEngine;


[CreateAssetMenu(fileName = "PickupSO", menuName = "MiniPunBall/Board/PickupSO", order = 0)]
public class PickupSO : ScriptableObject, IBoardData
{
    public string Id => name;

    [Header("Behaviors")]
    public MoveBehavior moveBehavior;

}
