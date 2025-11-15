using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterSO", menuName = "MiniPunBall/CharacterSO", order = 0)]
public class CharacterSO : ScriptableObject
{
    public int BaseHealth;
    public int BaseAttack;
    public BallSO BaseBallConfig;
    public int BaseBallsCount;

}
