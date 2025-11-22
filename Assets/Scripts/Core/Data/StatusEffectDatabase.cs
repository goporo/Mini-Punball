using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MiniPunBall/StatusEffectDatabase")]
public class StatusEffectDatabase : ScriptableObject
{
    public List<StatusEffectSO> statusEffects;
    public StatusEffectSO GetConfig(StatusEffectType type)
        => statusEffects.Find(c => c.Type == type);


}
