using UnityEngine;
using UnityEditor;

namespace OrionReed
{
  [CustomEditor(typeof(EntityGraphProjector))]
  [CanEditMultipleObjects]
  public class EntityGraphVolumeEditor : Editor
  {
    //SerializedProperty entityGraphVolume;

    void OnEnable()
    {
      //entityGraphVolume = serializedObject.FindProperty("lookAtPoint");
    }

    void OnDisable()
    {

    }

    public override void OnInspectorGUI()
    {
      base.DrawDefaultInspector();
      //serializedObject.Update();
      //EditorGUILayout.PropertyField(entityGraphVolume);
      //serializedObject.ApplyModifiedProperties();
    }
  }
}