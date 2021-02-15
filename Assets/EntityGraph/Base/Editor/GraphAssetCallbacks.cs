using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace OrionReed
{
  public static class GraphAssetCallbacks
  {
    [MenuItem("Assets/Create/Entity Graph", false, 10)]
    public static void CreateGraphPorcessor()
    {
      var graph = ScriptableObject.CreateInstance<EntityGraph>();
      ProjectWindowUtil.CreateAsset(graph, "EntityGraph.asset");
    }

    [OnOpenAsset(0)]
    public static bool OnBaseGraphOpened(int instanceID, int line)
    {
      var asset = EditorUtility.InstanceIDToObject(instanceID) as EntityGraph;

      if (asset != null)
      {
        //asset.Validate();
        EditorWindow.GetWindow<DefaultGraphWindow>().InitializeGraph(asset as EntityGraph);
        return true;
      }
      return false;
    }
  }
}