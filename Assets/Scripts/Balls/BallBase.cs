using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BallPhysics))]
public class BallBase : MonoBehaviour
{
    [SerializeField] private BallSO config;
    public BallSO Stats => config;
    public IStatusEffect StatusEffect => config.StatusEffect;

    private PlayerRunStats playerRunStats;
    private Vector3 initialDirection;
    public BallPhysics Physics { get; private set; }

    void Awake()
    {
        Physics = GetComponent<BallPhysics>();
    }

    public void Init(PlayerRunStats playerRunStats, Vector3 initialDirection)
    {
        this.playerRunStats = playerRunStats;
        this.initialDirection = initialDirection;
        Physics.Init(playerRunStats, config, initialDirection);
    }


}
