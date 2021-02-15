using GraphProcessor;
using UnityEngine;
using System.Collections.Generic;

namespace OrionReed
{
  [System.Serializable, NodeMenuItem("Operations/Remove from Sampler")]
  public class OperationRemoveSample : BaseEntityGraphNode
  {
    [Input(name = "Entities")]
    public EntityChunkMatrix entities;
    [Input(name = "Sampler")]
    public ISampler sampler;

    [Output(name = "Out")]
    public EntityChunkMatrix output;

    public override string name => "Remove Sampler";

    protected override void Process()
    {
      output = new EntityChunkMatrix();

      foreach (KeyValuePair<Coordinate, HashSet<string>> chunk in entities.AllChunkPairs)
      {
        System.Random RNG = graph.ChunkRandoms[chunk.Key];
        foreach (string entityID in chunk.Value)
        {
          if (!entities.TryGetEntity(entityID, out IEntity entity))
            continue;
          if (RNG.NextFloat(0, 1) < sampler.SamplePosition(entity.Position))
            output.AddEntity(entity);
        }
      }
    }
  }
}