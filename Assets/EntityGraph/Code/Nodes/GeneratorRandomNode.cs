using GraphProcessor;
using UnityEngine;

namespace OrionReed
{
  [System.Serializable, NodeMenuItem("Generators/Random")]
  public class GeneratorRandomNode : BaseEntityGraphNode
  {
    [Input(name = "Frequency"), ShowAsDrawer]
    public float frequency = 0.5f;

    [Input("Entity Settings")]
    public IEntitySampler entitySettings;

    [Output(name = "Out")]
    public EntityCollection output;

    public override string name => "Generate Random";

    protected override void Process()
    {
      output = new EntityCollection();

      foreach (Coordinate chunk in graph.CompleteRegion.EnumerateCoordinates())
      {
        CallCounter.Count(this);
        System.Random rng = RNG(chunk);
        Vector3 worldPos = Coordinate.WorldPosition(chunk);
        int numberOfPoints = (int)(Coordinate.scale / frequency);
        for (int i = 0; i < numberOfPoints; i++)
        {
          CallCounter.Count(this);
          Vector3 randomPos = rng.NextZeroedVector3(worldPos, worldPos + (Vector3.one * Coordinate.scale));
          output.AddEntity(new Entity(randomPos, entitySettings.GetWithRandom(rng)));
        }
      }
    }
  }
}