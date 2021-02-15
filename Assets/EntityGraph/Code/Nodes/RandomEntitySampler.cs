using System.Collections.Generic;
using GraphProcessor;
using UnityEngine;
using OrionReed;

namespace OrionReed
{
  [System.Serializable, NodeMenuItem("Entities/Random Size")]
  public class RandomEntitySampler : BaseEntityGraphNode, IEntitySampler
  {
    new public Color color = new Color(0.75f, 0.75f, 0.75f, 1f);
    public float min = 0.2f;
    public float max = 1.5f;

    [Output(name = "Out")]
    public IEntitySampler output;

    public override string name => "Random Entities";
    public override string layoutStyle => "EntitySettingsStyle";

    public IEntitySettingsData Get => new SimpleEntitySettings(color, max);

    public IEntitySettingsData GetWithRandom(System.Random rng)
    {
      return new SimpleEntitySettings(color, rng.NextFloat(min, max));
    }

    protected override void Process()
    {
      output = this;
    }
  }
}