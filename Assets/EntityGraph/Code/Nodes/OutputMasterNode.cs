using System.Collections.Generic;
using System.Linq;
using GraphProcessor;
using UnityEngine;
using UnityEditor;

namespace OrionReed
{
  [System.Serializable]
  public class OutputMasterNode : BaseEntityGraphNode
  {
    [Input, RequiredInput]
    public List<EntityCollection> inputs;

    private List<EntityCollection> values = new List<EntityCollection>();

    public override string name => "Output";
    public override bool deletable => false;
    public override string layoutStyle => "OutputStyle";

    protected override void Process()
    {
      EntityCollection result = EntityCollection.MergeIntoFirst(values);
      //graph.EntityCache = result;
      Debug.Log($"Result: {result.EntityCount} entities");
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