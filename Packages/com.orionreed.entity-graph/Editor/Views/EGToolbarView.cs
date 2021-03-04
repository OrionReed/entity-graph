using GraphProcessor;
using UnityEditor;
using UnityEngine.UIElements;
using Status = UnityEngine.UIElements.DropdownMenuAction.Status;
using System.Collections.Specialized;

namespace OrionReed
{
  public class EGToolbarView : ToolbarView
  {
    public EGToolbarView(BaseGraphView graphView) : base(graphView) { }
    private Label projectorCount;
    private EntityGraph graph;

    protected override void AddButtons()
    {
      graph = graphView.graph as EntityGraph;
      AddButton("Preview", () => Preview());
      AddButton("Apply", () => Apply());
      bool visualize = graphView.GetPinnedElementStatus<GizmoSettingsView>() != Status.Hidden;
      AddToggle("Visualization", visualize, (_) => graphView.ToggleView<GizmoSettingsView>());
      projectorCount = new Label($"Projectors in scene: {EntityGraph.ProjectorsInScene.Count}");
      Add(projectorCount);

      EntityGraph.ProjectorsInScene.CollectionChanged += UpdateProjectorCount;

      //bool exposedParamsVisible = graphView.GetPinnedElementStatus<ExposedParameterView>() != Status.Hidden;
      //AddToggle("Show Parameters", exposedParamsVisible, (_) => graphView.ToggleView<ExposedParameterView>(), false);

      AddButton("Center", graphView.ResetPositionAndZoom, false);
      AddButton("Show In Project", () => EditorGUIUtility.PingObject(graphView.graph), false);
    }
    private void Preview()
    {
      EntityGraphProcessor processor = new EntityGraphProcessor(graphView.graph as EntityGraph);
      processor.ProcessAllInstancesInScene();
    }
    private void Apply()
    {
      graph.ProjectCurrent();
    }

    private void UpdateProjectorCount(object _, NotifyCollectionChangedEventArgs __)
    {
      projectorCount.text = $"Projectors in scene: {EntityGraph.ProjectorsInScene.Count}";
    }
  }
}