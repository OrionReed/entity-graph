using System.Collections.Generic;
using System.Linq;
using GraphProcessor;
using UnityEngine;

namespace OrionReed
{
  [System.Serializable]
  public class OutputMasterNode : BaseEntityGraphNode
  {
    //[VisibleIf("someVar", 1)]
    public int seed;
    public Bounds bounds = new Bounds(new Vector3(), new Vector3(200, 10, 200));

    [Input, RequiredInput]
    public List<EntityChunkMatrix> inputs;

    private List<EntityChunkMatrix> values = new List<EntityChunkMatrix>();

    public override string name => "Output";
    public override bool deletable => false;
    public override string layoutStyle => "OutputStyle";

    protected override void Process()
    {
      graph.EntityCache = EntityChunkMatrix.MergeIntoFirst(values);
    }

    [CustomPortBehavior(nameof(inputs))]
    private IEnumerable<PortData> GetPortsForInputs(List<SerializableEdge> edges)
    {
      yield return new PortData { displayName = "In", displayType = typeof(EntityChunkMatrix), acceptMultipleEdges = true };
    }

    [CustomPortInput(nameof(inputs), typeof(EntityChunkMatrix), allowCast = true)]
    public void GetInputs(List<SerializableEdge> edges)
    {
      values = edges.ConvertAll(e => (EntityChunkMatrix)e.passThroughBuffer);
    }

    public void Visualize()
    {
      Utils.DrawBounds(bounds, Color.white);
    }
  }
}