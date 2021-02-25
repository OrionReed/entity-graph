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
    EntityGraphVisualizer viz;

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

    private void UpdateViz()
    {
      viz.Update();
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
      if (viz == null)
        viz = new EntityGraphVisualizer(graph as EntityGraph);
      if (graphView == null)
      {
        titleContent = new GUIContent(graph.name);
        graphView = new DefaultGraphView(this);
        graphView.Add(new CustomToolbarView(graphView));

        MiniMapView mmv = new MiniMapView(graphView);
        Vector2 mmvPos = mmv.GetPosition().position + new Vector2(5f, 25f);
        mmv.SetPosition(new Rect(mmvPos, Vector2.one));
        graphView.Add(mmv);
      }
      rootView.Add(graphView);
      SceneView.duringSceneGui += (_) => UpdateViz();
    }
  }
}