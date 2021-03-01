using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace OrionReed
{
  public static class EGAssetCallbacks
  {
    [MenuItem("Assets/Create/Entity Graph", false, 10)]
    public static void CreateGraphPorcessor()
    {
      var graph = ScriptableObject.CreateInstance<EntityGraph>();
      ProjectWindowUtil.CreateAsset(graph, "EntityGraph.asset");
    }

    [OnOpenAsset(0)]
    public static bool OnBaseGraphOpened(int instanceID, int _)
    {
      var asset = EditorUtility.InstanceIDToObject(instanceID) as EntityGraph;

      if (asset != null)
      {
        EditorWindow.GetWindow<EGWindow>().InitializeGraph(asset);
        return true;
      }
      return false;
    }
  }
}