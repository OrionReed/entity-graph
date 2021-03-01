using System.Collections.Generic;
using GraphProcessor;
using UnityEngine;

namespace OrionReed
{
  [System.Serializable, NodeMenuItem("Visualize")]
  public class VisualizeNode : BaseEntityGraphNode
  {
    public override string name => "Visualize";

    [Input("In")]
    EntityCollection viz;
  }
}