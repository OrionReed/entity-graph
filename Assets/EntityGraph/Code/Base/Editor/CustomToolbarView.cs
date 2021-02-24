using GraphProcessor;
using UnityEditor;
using Status = UnityEngine.UIElements.DropdownMenuAction.Status;

namespace OrionReed
{
  public class CustomToolbarView : ToolbarView
  {
    public CustomToolbarView(BaseGraphView graphView) : base(graphView) { }

    protected override void AddButtons()
    {
      AddButton("Run", () => ProcessGraph());
      AddButton("Clear", () => ClearGraph());
      AddButton("Center", graphView.ResetPositionAndZoom);

      bool exposedParamsVisible = graphView.GetPinnedElementStatus<ExposedParameterView>() != Status.Hidden;
      AddToggle("Show Parameters", exposedParamsVisible, (_) => graphView.ToggleView<ExposedParameterView>());

      bool callCountEnabled = CallCounter.Enabled;
      AddToggle("Count Calls", callCountEnabled, (_) => CallCounter.Enabled = !CallCounter.Enabled, false);
      AddButton("Benchmark", () => BenchmarkStatic.Run(), false);

      AddButton("Show In Project", () => EditorGUIUtility.PingObject(graphView.graph), false);
    }
    private void ProcessGraph()
    {
      EntityGraphProcessor processor = new EntityGraphProcessor(graphView.graph as EntityGraph);
      processor.ProcessEntityGraph();
    }
    private void ClearGraph()
    {
      EntityGraph graph = graphView.graph as EntityGraph;
      graph.Clear();
    }
  }
}