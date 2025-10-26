using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelDatabaseSO", menuName = "MiniPunBall/LevelDatabaseSO", order = 0)]
public class LevelDatabaseSO : ScriptableObject
{
    public List<LevelSO> levels;

    public LevelSO GetLevel(int index)
    {
        if (index < 0 || index >= levels.Count)
        {
            Debug.LogError("Level index out of range: " + index);
            return null;
        }
        return levels[index];
    }


}
