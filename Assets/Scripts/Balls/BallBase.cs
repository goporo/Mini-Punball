using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BallPhysics))]
public class BallBase : MonoBehaviour
{
    [SerializeField] private BallSO config;
    public BallSO Stats => config;
    public BallPhysics Physics { get; private set; }

    void Awake()
    {
        Physics = GetComponent<BallPhysics>();
    }

    public void Init(PlayerRunStats playerRunStats, Vector3 initialPos, Vector3 initialDirection, Quaternion initialRotation)
    {
        transform.position = initialPos;
        Physics.Init(playerRunStats, initialDirection, initialRotation);
    }


}
