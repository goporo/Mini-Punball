using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MiniPunBall/CharacterSO")]
public class CharacterSO : ScriptableObject
{
    public int BaseHealth;
    public int BaseAttack;
    public BallSO BaseBallConfig;
    public int BaseBallsCount;

}
