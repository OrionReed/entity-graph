using GraphProcessor;
using UnityEngine;
using System;
using System.Collections.Generic;

namespace OrionReed
{
  [Serializable, NodeMenuItem("Operations/Remove In Range")]
  public class OperationRemoveInRange : BaseEntityGraphNode
  {
    [Input(name = "Radius Source")]
    public EntityCollection radiusSource;
    [Input(name = "Apply To")]
    public EntityCollection applyTo;

    [Input(name = "Radius"), ShowAsDrawer]
    public float radius = 1f;

    [Output(name = "Out")]
    public EntityCollection output;

    public override string name => "Remove In Range";

    protected override void Process()
    {
      output = EntityCollection.SubtractLayers(radiusSource, applyTo, radius);
    }
  }
}