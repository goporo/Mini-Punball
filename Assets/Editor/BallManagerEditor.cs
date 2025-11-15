using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BallManager))]
public class BallManagerEditor : Editor
{
  public override void OnInspectorGUI()
  {
    DrawDefaultInspector();

    BallManager manager = (BallManager)target;
    if (GUILayout.Button("Force Balls Return"))
    {
      manager.ForceBallsReturn();
    }
  }
}
