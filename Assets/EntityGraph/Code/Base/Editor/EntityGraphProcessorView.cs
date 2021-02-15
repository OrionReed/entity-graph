using GraphProcessor;
using UnityEngine.UIElements;

namespace OrionReed
{
  public class EntityGraphProcessorView : PinnedElementView
  {
    private EntityGraphProcessor processor;

    public EntityGraphProcessorView()
    {
      title = "Process EntityGraph";
    }

    protected override void Initialize(BaseGraphView graphView)
    {
      processor = new EntityGraphProcessor(graphView.graph as EntityGraph);

      graphView.computeOrderUpdated += processor.UpdateComputeOrder;

      Button b = new Button(OnPlay) { name = "ActionButton", text = "Run" };

      content.Add(b);
    }

    private void OnPlay()
    {
      processor.ProcessEntityGraph();
    }
  }
}