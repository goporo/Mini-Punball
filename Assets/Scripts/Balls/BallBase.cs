using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BallPhysics))]
public abstract class BallBase : MonoBehaviour
{
    [SerializeField] private BallSO ballStats;
    public BallSO BallStats => ballStats;
    public IStatusEffect StatusEffect => ballStats.statusEffect;

    private PlayerRunStats playerRunStats;
    private Vector3 initialDirection;


    public void Init(PlayerRunStats playerRunStats, Vector3 initialDirection)
    {
        this.playerRunStats = playerRunStats;
        this.initialDirection = initialDirection;
        var ballPhysics = GetComponent<BallPhysics>();
        ballPhysics?.Init(playerRunStats, ballStats, initialDirection);
    }

    void Awake()
    {
    }
    void Start()
    {

    }

    void Update()
    {

    }

}

public struct DamageContext
{
    public GameObject source;
    public int amount;
    public IStatusEffect statusEffect;
}