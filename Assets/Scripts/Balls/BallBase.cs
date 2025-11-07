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
    public DamageType damageType;

    public DamageContext(int amount, DamageType damageType)
    {
        this.amount = amount;
        this.damageType = damageType;
    }
}