using GraphProcessor;
using UnityEditor;
using UnityEngine.UIElements;

namespace OrionReed
{
  [CustomEditor(typeof(EntityGraph), true)]
  public class EGAssetInspector : GraphInspector
  {
    protected override void CreateInspector()
    {
      base.CreateInspector();

      root.Add(new Button(() => EditorWindow.GetWindow<EGWindow>().InitializeGraph(target as EntityGraph))
      {
        text = "Open Entity Graph Window"
      });
    }
  }
}