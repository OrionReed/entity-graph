using GraphProcessor;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace OrionReed
{
  public class EGWindow : BaseGraphWindow
  {
    private static Texture2D windowIcon;
    private VisualElement noAsset;

    [MenuItem("Window/Entity Graph")]
    public static BaseGraphWindow OpenNewEditor()
    {
      var graphWindow = CreateWindow<EGWindow>();
      graphWindow.NoAssetView();

      graphWindow.Show();
      return graphWindow;
    }

    private void OnCreateAsset()
    {
      string filePath = EditorUtility.SaveFilePanelInProject("", "New EntityGraph", "asset", "Create new Entity Graph");
      if (!string.IsNullOrEmpty(filePath))
      {
        EntityGraph newGraph = CreateInstance<EntityGraph>();
        AssetDatabase.CreateAsset(newGraph, filePath);
        InitializeGraph(AssetDatabase.LoadAssetAtPath<EntityGraph>(filePath));
      }
      if (noAsset != null)
        rootView.Remove(noAsset);
    }

    public static BaseGraphWindow Open()
    {
      var graphWindow = GetWindow<EGWindow>();

      graphWindow.Show();

      return graphWindow;
    }

    protected override void OnEnable()
    {
      if (windowIcon == null)
        windowIcon = GetWindowIcon();
      base.OnEnable();
      if (!graph)
        NoAssetView();
    }

    protected override void OnDisable()
    {
      if (graph != null && graphView != null)
        graphView.SaveGraphToDisk();
    }

    protected override void OnDestroy()
    {
      graphView?.Dispose();
    }

    public override void OnGraphDeleted()
    {
      if (graph != null)
        rootView.Remove(graphView);

      NoAssetView();
      graphView = null;
    }

    protected override void InitializeWindow(BaseGraph graph)
    {
      if (graphView == null)
      {
        if (noAsset != null)
          rootView.Remove(noAsset);

        SetTitle(graph.name);
        graphView = new EGView(this);
        graphView.Add(new EGToolbarView(graphView));
      }
      rootView.Add(graphView);
      SceneView.duringSceneGui += (_) => DrawProjectors();
    }

    private void SetTitle(string name)
    {
      if (windowIcon != null)
      {
        titleContent = new GUIContent(name, windowIcon);
        return;
      }
      titleContent = new GUIContent(name);
    }

    private Texture2D GetWindowIcon()
    {
      return (Texture2D)AssetDatabase.LoadAssetAtPath("Packages/com.orionreed.entity-graph/Editor/Icons/EG_Window.png", typeof(Texture2D));
    }


    private void NoAssetView()
    {
      SetTitle("Empty Entity Graph");
      noAsset = new Label("\n\n\nTo begin using Entity Graph, create a new Entity Graph Asset.\n(or double-click an existing Entity Graph in the project view)") { name = "no-asset" };
      noAsset.style.position = Position.Absolute;
      noAsset.style.unityTextAlign = TextAnchor.MiddleCenter;
      noAsset.style.left = new StyleLength(40f);
      noAsset.style.right = new StyleLength(40f);
      noAsset.style.top = new StyleLength(40f);
      noAsset.style.bottom = new StyleLength(140f);
      noAsset.style.fontSize = new StyleLength(12f);
      noAsset.style.color = Color.white * 0.75f;

      var createButton = new Button(OnCreateAsset) { text = "Create new Entity Graph" };
      createButton.style.position = Position.Absolute;
      createButton.style.alignSelf = Align.Center;
      noAsset.Add(createButton);
      rootView.Add(noAsset);
    }

    private void DrawProjectors()
    {
      foreach (EntityGraphProjector projector in EntityGraph.ProjectorsInScene)
      {
        if (projector.Graph == null) // we need to add a graph
        {
          EGProjectorUtil.GraphMissing(EGProjectorUtil.ColDefault, projector.Bounds);
          continue;
        }
        if (EntityGraph.debugDrawBounds && EntityGraph.debugDrawChunks) // we limit chunks to only show on face of bounds
        {
          EGProjectorUtil.DrawBoundsWire(EGProjectorUtil.ColDefault, projector.Bounds);
          EGProjectorUtil.DrawClippedChunks(projector.Dirty ? EGProjectorUtil.ColDirty * 0.65f : EGProjectorUtil.ColDefault * 0.65f, projector.Bounds);
          continue;
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