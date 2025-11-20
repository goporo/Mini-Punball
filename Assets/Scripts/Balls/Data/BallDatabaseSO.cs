using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MiniPunBall/Ball/BallDatabaseSO")]
public class BallDatabaseSO : ScriptableObject
{
    public List<BallSO> balls;
    public BallSO GetConfig(BallType type)
        => balls.Find(c => c.BallType == type);


}
