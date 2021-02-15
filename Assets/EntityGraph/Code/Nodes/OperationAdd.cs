using GraphProcessor;
using UnityEngine;

namespace OrionReed
{
  [System.Serializable, NodeMenuItem("Operations/Add")]
  public class OperationAdd : BaseEntityGraphNode
  {
    [Input(name = "Input A")]
    public EntityChunkMatrix a;
    [Input(name = "Input B")]
    public EntityChunkMatrix b;

    [Output(name = "Out")]
    public EntityChunkMatrix output;

    public override string name => "Add";

    protected override void Process()
    {
      output = EntityChunkMatrix.MergeIntoFirst(a, b);
    }
  }
}