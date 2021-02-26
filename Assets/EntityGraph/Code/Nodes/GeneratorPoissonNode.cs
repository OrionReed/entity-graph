using System.Collections.Generic;
using GraphProcessor;
using UnityEngine;

namespace OrionReed
{
  [System.Serializable, NodeMenuItem("Generators/Poisson")]
  public class GeneratorPoissonNode : BaseEntityGraphNode
  {
    [Input(name = "Disk Radius"), ShowAsDrawer]
    public float radius = 5f;

    [Input("Entity Settings")]
    public IEntitySampler entitySettings;

    [Output(name = "Out")]
    public EntityCollection output;

    public override string name => "Generate Poisson";

    protected override void Process()
    {
      output = new EntityCollection();

      foreach (Coordinate chunk in graph.GetCurrentRegion().EnumerateCoordinates())
      {
        System.Random rng = RNG(chunk);
        foreach (Vector2 sample in PoissonSampler.GenerateSamples(rng, radius, Coordinate.scale, Coordinate.scale))
        {
          Entity e = new Entity(MapToChunkSpace(sample, chunk), entitySettings.GetWithRandom(rng));
          output.AddEntity(e);
        }
      }

      static Vector2 MapToChunkSpace(Vector2 vector2, Coordinate coordinate)
      {
        return new Vector2(vector2.x + Coordinate.WorldPosition(coordinate).x, vector2.y + Coordinate.WorldPosition(coordinate).y);
      }
    }
  }
}