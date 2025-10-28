using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BallDatabaseSO", menuName = "MiniPunBall/BallDatabaseSO", order = 0)]
public class BallDatabaseSO : ScriptableObject
{
    public List<BallSO> balls;

    public BallSO GetConfig(BallType type)
        => balls.Find(c => c.BallType == type);


}
