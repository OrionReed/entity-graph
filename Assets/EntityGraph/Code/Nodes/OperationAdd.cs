using GraphProcessor;
using UnityEngine;

namespace OrionReed
{
  [System.Serializable, NodeMenuItem("Operations/Add")]
  public class OperationAdd : BaseEntityGraphNode
  {
    [Input(name = "Input A")]
    public EntityCollection a;
    [Input(name = "Input B")]
    public EntityCollection b;

    [Output(name = "Out")]
    public EntityCollection output;

    public override string name => "Add";

    protected override void Process()
    {
      output = EntityCollection.MergeIntoFirst(a, b);
    }
  }
}