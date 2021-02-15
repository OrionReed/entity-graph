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
    public override string layoutStyle => "EntityCollectionStyle";

    protected override void Process()
    {
      output = new EntityCollection(graph.OutputMasterNode.bounds);

      foreach (Coordinate chunk in output.AllChunkCoordinates)
      {
        System.Random rng = RNG(chunk);
        foreach (Vector2 sample in PoissonSampler.GenerateSamples(rng, radius, Coordinate.scale, Coordinate.scale))
        {
          Entity e = new Entity(MapToChunkSpace(sample, chunk), entitySettings.GetWithRandom(rng));
          output.AddEntity(e);
        }
      }
      static Vector3 MapToChunkSpace(Vector2 vector2, Coordinate coordinate)
      {
        return new Vector3(vector2.x + Coordinate.WorldPosition(coordinate).x, 0, vector2.y + Coordinate.WorldPosition(coordinate).z);
      }
    }
  }
}