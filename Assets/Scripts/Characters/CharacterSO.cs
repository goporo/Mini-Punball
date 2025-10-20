using UnityEngine;

[CreateAssetMenu(fileName = "CharacterSO", menuName = "MiniPunBall/CharacterSO", order = 0)]
public class CharacterSO : ScriptableObject
{
    public int BaseAttack;
    public BallType BaseBallType;
    [TextArea] public string Description;

}
