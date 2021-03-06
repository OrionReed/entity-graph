﻿using GraphProcessor;
using UnityEngine;

namespace OrionReed
{
  [System.Serializable, NodeMenuItem("Generators/Random")]
  public class GeneratorRandomNode : BaseEntityGraphNode
  {
    [Input(name = "Frequency"), ShowAsDrawer]
    public float frequency = 0.5f;

    [Output(name = "Out")]
    public EntityCollection output;

    public override string name => "Generate Random";

    protected override void Process()
    {
      output = new EntityCollection();

      foreach (Coordinate chunk in graph.GetCurrentRegion().EnumerateCoordinates())
      {
        System.Random rng = RNG(chunk);
        Vector2 worldPos = Coordinate.WorldPosition(chunk);
        int numberOfPoints = (int)(Coordinate.scale / frequency);
        for (int i = 0; i < numberOfPoints; i++)
        {
          Vector2 randomPos = rng.NextVector2(worldPos, worldPos + (Vector2.one * Coordinate.scale));
          output.AddEntity(new Entity(randomPos));
        }
      }
    }
  }
}