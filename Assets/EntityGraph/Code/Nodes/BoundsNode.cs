using System.Collections.Generic;
using System.Linq;
using GraphProcessor;
using UnityEngine;
using UnityEditor;

namespace OrionReed
{
  [System.Serializable]
  //[NodeMenuItem("Bounds")]
  public class BoundsNode : BaseEntityGraphNode
  {
    public Bounds bounds = new Bounds(new Vector3(), new Vector3(200, 10, 200));

    [Output]
    private Bounds output;

    public override string name => "Bounds";

    protected override void Process()
    {
      output = bounds;
    }
  }
}