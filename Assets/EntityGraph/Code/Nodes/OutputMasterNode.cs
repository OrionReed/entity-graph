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
    //[VisibleIf("someVar", 1)]
    public int seed;
    public Bounds bounds = new Bounds(new Vector3(), new Vector3(200, 10, 200));

    [Input, RequiredInput]
    public List<EntityCollection> inputs;

    private List<EntityCollection> values = new List<EntityCollection>();

    public override string name => "Output";
    public override bool deletable => false;
    public override string layoutStyle => "OutputStyle";

    protected override void Process()
    {
      graph.EntityCache = EntityCollection.MergeIntoFirst(values);
      Debug.LogWarning($"Entities: {graph.EntityCache.EntityCount}");
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

    public void Visualize()
    {
      Util.DrawBounds(bounds, Color.white / 2);
    }
  }
}