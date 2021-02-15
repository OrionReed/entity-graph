using System.Collections;
using System.Collections.Generic;
using GraphProcessor;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace OrionReed
{
  [CustomEditor(typeof(EntityGraph), true)]
  public class GraphAssetInspector : GraphInspector
  {
    protected override void CreateInspector()
    {
      base.CreateInspector();

      root.Add(new Button(() => EditorWindow.GetWindow<DefaultGraphWindow>().InitializeGraph(target as EntityGraph))
      {
        text = "Open Entity Graph Window"
      });
    }
  }
}