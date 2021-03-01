using GraphProcessor;
using UnityEditor;
using Status = UnityEngine.UIElements.DropdownMenuAction.Status;

namespace OrionReed
{
  public class EGToolbarView : ToolbarView
  {
    public EGToolbarView(BaseGraphView graphView) : base(graphView) { }

    protected override void AddButtons()
    {
      AddButton("Preview", () => ProcessAllInScene());
      AddButton("Apply", () => Apply());
      bool visualize = graphView.GetPinnedElementStatus<GizmoSettingsView>() != Status.Hidden;
      AddToggle("Visualization", visualize, (_) => graphView.ToggleView<GizmoSettingsView>());

      //bool exposedParamsVisible = graphView.GetPinnedElementStatus<ExposedParameterView>() != Status.Hidden;
      //AddToggle("Show Parameters", exposedParamsVisible, (_) => graphView.ToggleView<ExposedParameterView>(), false);

      AddButton("Center", graphView.ResetPositionAndZoom, false);
      AddButton("Show In Project", () => EditorGUIUtility.PingObject(graphView.graph), false);
    }
    private void ProcessAllInScene()
    {
      EntityGraphProcessor processor = new EntityGraphProcessor(graphView.graph as EntityGraph);
      processor.ProcessAllInstancesInScene();
    }
    private void Apply()
    {
      //EntityGraph graph = graphView.graph as EntityGraph;
    }
  }
}