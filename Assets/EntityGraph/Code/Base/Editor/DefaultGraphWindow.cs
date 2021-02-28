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

    protected override void OnDisable()
    {
      if (graph != null && graphView != null)
        graphView.SaveGraphToDisk();
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
        titleContent = new GUIContent(graph.name);
        graphView = new DefaultGraphView(this);
        graphView.Add(new CustomToolbarView(graphView));

        MiniMapView mmv = new MiniMapView(graphView);
        Vector2 mmvPos = mmv.GetPosition().position + new Vector2(5f, 25f);
        mmv.SetPosition(new Rect(mmvPos, Vector2.one));
        graphView.Add(mmv);
      }
      rootView.Add(graphView);
      SceneView.duringSceneGui += (_) => DrawProjectors();
    }

    private void DrawProjectors()
    {
      foreach (EntityGraphProjector projector in EntityGraph.ProjectorsInScene)
      {
        if (projector.Graph == null) // we need to add a graph
        {
          EGProjectorUtil.DrawBoundsDotted(EGProjectorUtil.ColDefault, projector.Bounds);
          return;
        }
        if (EntityGraph.debugDrawBounds && EntityGraph.debugDrawChunks) // we limit chunks to only show on face of bounds
        {
          EGProjectorUtil.DrawBoundsWire(EGProjectorUtil.ColDefault, projector.Bounds);
          EGProjectorUtil.DrawClippedChunks(projector.Dirty ? EGProjectorUtil.ColDirty * 0.65f : EGProjectorUtil.ColDefault * 0.65f, projector.Bounds);
          return;
        }
        if (EntityGraph.debugDrawBounds) // only showing bounds
        {
          EGProjectorUtil.DrawBoundsWire(EGProjectorUtil.ColDefault, projector.Bounds);
        }
        if (EntityGraph.debugDrawChunks) // only showing chunks
        {
          EGProjectorUtil.DrawChunks(projector.Dirty ? EGProjectorUtil.ColDirty : EGProjectorUtil.ColDefault, projector.Bounds);
          EGProjectorUtil.DrawTop(EGProjectorUtil.ColDefault * 0.65f, projector.Bounds);
        }
      }
    }
  }
}