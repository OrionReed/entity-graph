using GraphProcessor;
using UnityEngine;
using System.Collections.Generic;

namespace OrionReed
{
  [System.Serializable, NodeMenuItem("Operations/Generate Nearby")]
  public class OperationAddNearby : BaseEntityGraphNode
  {
    [Input(name = "In")]
    public EntityCollection input;

    [Input(name = "Number to Add"), ShowAsDrawer]
    public int numberToAdd = 1;
    [Input(name = "Max Distance"), ShowAsDrawer]
    public float maxDistance = 5f;

    [Input("Entity Settings")]
    public IEntitySettingsSampler entitySettings;

    [Output(name = "Out")]
    public EntityCollection output;

    public override string name => "Generate Nearby";

    protected override void Process()
    {
      output = new EntityCollection();
      foreach (KeyValuePair<Coordinate, HashSet<string>> chunk in input.AllChunkPairs)
      {
        System.Random rng = RNG(chunk.Key);
        foreach (string entityID in chunk.Value)
        {
          for (int x = 0; x < numberToAdd; x++)
          {
            if (!input.TryGetEntity(entityID, out IEntity entity)) continue;
            output.AddEntity(new Entity(entity.Position + rng.GetPointInCircle(maxDistance), entitySettings.Get()));
          }
        }
      }
    }
  }
}