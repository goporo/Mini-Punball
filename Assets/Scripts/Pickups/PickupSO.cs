using UnityEngine;


[CreateAssetMenu(menuName = "MiniPunBall/Board/PickupSO")]
public class PickupSO : ScriptableObject, IBoardData
{
    public string Id => name;

    [Header("Behaviors")]
    public MoveBehavior moveBehavior;

}
