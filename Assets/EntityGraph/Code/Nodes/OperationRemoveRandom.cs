using GraphProcessor;
using UnityEngine;
using System.Collections.Generic;

namespace OrionReed
{
  [System.Serializable, NodeMenuItem("Operations/Remove at Random")]
  public class OperationRemoveRandom : BaseEntityGraphNode
  {
    [Input(name = "In")]
    public EntityCollection input;

    [Input(name = "Survival Probability"), ShowAsDrawer]
    public int chanceOfSurvival;

    [Output(name = "Out")]
    public EntityCollection output;

    public override string name => "Remove Random";

    protected override void Process()
    {
      output = new EntityCollection();

      foreach (KeyValuePair<Coordinate, HashSet<string>> chunk in input.AllChunkPairs)
      {
        System.Random RNG = graph.ChunkRandoms[chunk.Key];
        foreach (string entityID in chunk.Value)
        {
          if (RNG.Next(0, 100) < chanceOfSurvival)
          {
            if (!input.TryGetEntity(entityID, out IEntity entity)) continue;
            output.AddEntity(entity);
          }
        }
      }
    }
  }
}