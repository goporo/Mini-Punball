using UnityEditor;
using UnityEngine;
using System.IO;

public class PlayerSkillSOAssetProcessor : AssetModificationProcessor
{
  static void OnWillCreateAsset(string path)
  {
    if (!path.EndsWith(".asset")) return;
    var assetPath = path.Replace(".meta", "");
    var asset = AssetDatabase.LoadAssetAtPath<PlayerSkillSO>(assetPath);
    if (asset != null)
    {
      // Always assign a new skillId when asset is created
      var so = asset as PlayerSkillSO;
      var soType = typeof(PlayerSkillSO);
      var field = soType.GetField("skillId", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
      if (field != null)
      {
        field.SetValue(so, System.Guid.NewGuid().ToString());
        EditorUtility.SetDirty(so);
        AssetDatabase.SaveAssets();
      }
    }
  }
}
