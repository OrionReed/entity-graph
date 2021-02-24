using GraphProcessor;
using UnityEngine;
using System.Collections.Generic;

namespace OrionReed
{
  [System.Serializable, NodeMenuItem("Viz/Map")]
  public class VizMap : BaseEntityGraphNode
  {
    public override string name => "Viz Map";

    [Input(name = "In")]
    public Map input;

    protected override void Process()
    {
      graph.Map = input;
    }
  }
}