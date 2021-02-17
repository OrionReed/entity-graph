using System.Collections.Generic;
using GraphProcessor;
using UnityEngine;

namespace OrionReed
{
  [System.Serializable, NodeMenuItem("Generators/Poisson New")]
  public class GeneratorPoissonNewNode : BaseEntityGraphNode
  {
    [Input(name = "Disk Radius"), ShowAsDrawer]
    public float radius = 5f;

    [Input("Entity Settings")]
    public IEntitySampler entitySettings;

    [Output(name = "Out")]
    public EntityCollection output;

    public override string name => "Generate Poisson New";

    protected override void Process()
    {
      System.Random rng = RNG(new Coordinate(0, 0));
      Region region = graph.CompleteRegion;
      output = PoissonSamplerNew.GenerateSamples(rng, radius, region, entitySettings.Get);
    }
  }
}