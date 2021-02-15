using System.Collections;
using System.Collections.Generic;
using GraphProcessor;
using UnityEditor;
using UnityEngine;

namespace OrionReed
{
  public class DefaultGraphWindow : BaseGraphWindow
  {
    private EntityGraph tmpGraph;

    [MenuItem("Window/Entity Graph")]
    public static BaseGraphWindow OpenWithTmpGraph()
    {
      var graphWindow = CreateWindow<DefaultGraphWindow>();

      // When the graph is opened from the window, we don't save the graph to disk
      graphWindow.tmpGraph = ScriptableObject.CreateInstance<EntityGraph>();
      graphWindow.tmpGraph.hideFlags = HideFlags.HideAndDontSave;
      graphWindow.InitializeGraph(graphWindow.tmpGraph);

      graphWindow.Show();

      return graphWindow;
    }

    public static BaseGraphWindow Open()
    {
      var graphWindow = GetWindow<DefaultGraphWindow>();

      graphWindow.Show();

      return graphWindow;
    }

    protected override void OnDestroy()
    {
      graphView?.Dispose();
      DestroyImmediate(tmpGraph);
    }

    public override void OnGraphDeleted()
    {
      if (graph != null)
        rootView.Remove(graphView);

      graphView = null;
    }

    protected override void InitializeWindow(BaseGraph graph)
    {
      if (graphView == null)
      {
        titleContent = new GUIContent("Entity Graph");
        graphView = new DefaultGraphView(this);
        graphView.Add(new CustomToolbarView(graphView));
        //graphView.Add(new MiniMapView(graphView));
      }
      rootView.Add(graphView);
    }
  }
}