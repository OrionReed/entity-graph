using System.Collections.Generic;
using System.Linq;
using GraphProcessor;
using UnityEngine;

namespace OrionReed
{
  [System.Serializable]
  public class OutputMasterNode : BaseEntityGraphNode
  {
    public int seed;
    [Space]
    public Vector3 origin = Vector3.zero;
    //[VisibleIf("seed", 1)]
    public Vector3 bounds = new Vector3(100, 10, 100);

    [Input, RequiredInput]
    public List<EntityChunkMatrix> inputs;

    private List<EntityChunkMatrix> values = new List<EntityChunkMatrix>();

    public override string name => "Master Output";
    public override bool deletable => false;
    public override string layoutStyle => "MasterStyle";

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
      Utils.DrawBounds(origin, bounds, Color.white / 2);
    }
  }
}