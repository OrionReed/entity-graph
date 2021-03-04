using System.Collections.Generic;
using GraphProcessor;
using UnityEngine;

namespace OrionReed
{
  [System.Serializable]
  public class OutputNode : BaseEntityGraphNode
  {
    [Input, RequiredInput]
    public List<EntityCollection> inputs;

    private List<EntityCollection> values = new List<EntityCollection>();

    public override string name => "Output";
    public override bool deletable => false;
    public override string layoutStyle => "OutputStyle";

    protected override void Process()
    {

      graph.AddProcessedResult(EntityCollection.MergeIntoFirst(values));
    }

    [CustomPortBehavior(nameof(inputs))]
    private IEnumerable<PortData> GetPortsForInputs(List<SerializableEdge> _)
    {
      yield return new PortData { displayName = "In", displayType = typeof(EntityCollection), acceptMultipleEdges = true };
    }

    [CustomPortInput(nameof(inputs), typeof(EntityCollection), allowCast = true)]
    public void GetInputs(List<SerializableEdge> edges)
    {
      values = edges.ConvertAll(e => (EntityCollection)e.passThroughBuffer);
    }
  }
}