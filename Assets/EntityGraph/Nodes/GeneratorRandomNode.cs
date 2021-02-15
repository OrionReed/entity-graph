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
    public IEntitySettingsSampler entitySettings;

    [Output(name = "Out")]
    public EntityChunkMatrix output;

    public override string name => "Generate Random";
    public override string layoutStyle => "EntityCollectionStyle";


    protected override void Process()
    {
      output = new EntityChunkMatrix(graph.OutputMasterNode.origin, graph.OutputMasterNode.bounds);

      foreach (Coordinate chunk in output.AllChunkCoordinates)
      {
        CallCounter.Count(this);
        System.Random rng = RNG(chunk);
        Vector3 worldPos = Coordinate.GetWorldPositionFromCoordinate(chunk);
        int numberOfPoints = (int)(EntityChunkMatrix.chunkSize / frequency);
        for (int i = 0; i < numberOfPoints; i++)
        {
          CallCounter.Count(this);
          Vector3 randomPos = rng.NextZeroedVector3(worldPos, worldPos + (Vector3.one * EntityChunkMatrix.chunkSize));
          output.AddEntity(new Entity(randomPos, entitySettings.Get()));
        }
      }
    }
  }
}