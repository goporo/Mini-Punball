using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSkillSO", menuName = "MiniPunBall/PlayerSkillSO", order = 0)]
public class PlayerSkillSO : ScriptableObject
{
  public int skillID;
  public string skillName;
  [TextArea]
  public string description;
  public Sprite icon;
}