using UnityEngine;


[CreateAssetMenu(menuName = "MiniPunBall/Board/ObstacleSO")]
public class ObstacleSO : ScriptableObject, IBoardData
{
    public string Id => name;


}
