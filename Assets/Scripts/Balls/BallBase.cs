using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BallPhysics))]
public class BallBase : MonoBehaviour
{
    [SerializeField] private BallSO config;
    public BallSO Stats => config;
    public IStatusEffect StatusEffect => config.statusEffect;

    private PlayerRunStats playerRunStats;
    private Vector3 initialDirection;


    public void Init(PlayerRunStats playerRunStats, Vector3 initialDirection)
    {
        this.playerRunStats = playerRunStats;
        this.initialDirection = initialDirection;
        var ballPhysics = GetComponent<BallPhysics>();
        ballPhysics?.Init(playerRunStats, config, initialDirection);
    }


}

public struct DamageContext
{
    public int amount;
    public IStatusEffect statusEffect;

    public DamageContext(int amount, IStatusEffect statusEffect)
    {
        this.amount = amount;
        this.statusEffect = statusEffect;
    }
}