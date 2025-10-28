using UnityEngine;


[CreateAssetMenu(fileName = "ObstacleSO", menuName = "MiniPunBall/Board/ObstacleSO", order = 0)]
public class ObstacleSO : ScriptableObject, IBoardData
{
    public string Id => name;


}
