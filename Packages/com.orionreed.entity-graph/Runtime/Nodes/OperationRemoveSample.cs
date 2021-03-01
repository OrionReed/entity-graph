using GraphProcessor;
using UnityEngine;
using System.Collections.Generic;

namespace OrionReed
{
  [System.Serializable, NodeMenuItem("Operations/Remove using Sampler")]
  public class OperationRemoveSample : BaseEntityGraphNode
  {
    [Input(name = "Entities")]
    public EntityCollection entities;
    [Input(name = "Sampler")]
    public IPositionSampler sampler;

    [Output(name = "Out")]
    public EntityCollection output;

    public override string name => "Remove Sampler";

    protected override void Process()
    {
      output = new EntityCollection();

      foreach (KeyValuePair<Coordinate, HashSet<string>> chunk in entities.AllChunks)
      {
        System.Random RNG = graph.ChunkRandoms[chunk.Key];
        foreach (string entityID in chunk.Value)
        {
          if (!entities.TryGetEntity(entityID, out IEntity entity))
            continue;
          if (RNG.NextFloat(0, 1) > sampler.SamplePosition(entity.Position))
            output.AddEntity(entity);
        }
      }
    }
  }
}