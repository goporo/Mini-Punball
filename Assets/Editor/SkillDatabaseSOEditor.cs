using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SkillDatabaseSO))]
public class SkillDatabaseSOEditor : Editor
{
  public override void OnInspectorGUI()
  {
    DrawDefaultInspector();

    SkillDatabaseSO db = (SkillDatabaseSO)target;
    if (GUILayout.Button("Regenerate All Skill IDs"))
    {
      int count = 0;
      foreach (var skill in db.skills)
      {
        if (skill != null)
        {
          var soType = typeof(PlayerSkillSO);
          var field = soType.GetField("skillId", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
          if (field != null)
          {
            field.SetValue(skill, System.Guid.NewGuid().ToString());
            EditorUtility.SetDirty(skill);
            count++;
          }
        }
      }
      AssetDatabase.SaveAssets();
      Debug.Log($"Regenerated IDs for {count} skills in database.");
    }
  }
}
