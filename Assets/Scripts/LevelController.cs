using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    void OnEnable()
    {
        BallPhysics.OnBallReturned += HandleBallReturn;
    }

    void OnDisable()
    {
        BallPhysics.OnBallReturned -= HandleBallReturn;
    }

    void HandleBallReturn(Vector3 ballPos)
    {
    }
}
