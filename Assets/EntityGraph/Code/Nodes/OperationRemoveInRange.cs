using GraphProcessor;
using UnityEngine;
using System;
using System.Collections.Generic;

namespace OrionReed
{
  [Deprecated(typeof(OperationPoissonRemoveNode))]
  public class OperationPoissonRemoveNode { }

  [System.Serializable, NodeMenuItem("Operations/Remove In Range")]
  public class OperationRemoveInRange : BaseEntityGraphNode
  {
    [Input(name = "A")]
    public EntityChunkMatrix a;
    [Input(name = "B")]
    public EntityChunkMatrix b;

    [Input(name = "Radius"), ShowAsDrawer]
    public float radius = 1f;

    [Output(name = "Out")]
    public EntityChunkMatrix output;

    public override string name => "Remove In Range";

    protected override void Process()
    {
      output = b.Copy();

      foreach (KeyValuePair<Coordinate, HashSet<string>> chunk in a.AllChunkPairs)
      {
        System.Random rng = RNG(chunk.Key);
        foreach (string entityID in chunk.Value)
        {
          if (!a.TryGetEntity(entityID, out IEntity entity)) continue;
          output.RemoveInRadiusOf(entity.Position, radius);
        }
      }
    }
  }
}