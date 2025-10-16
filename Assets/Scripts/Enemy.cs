using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public float health = 100f;
    public float maxHealth = 100f;
    public bool IsAlive => health > 0;
    public Vector3 Position => transform.position;


}
