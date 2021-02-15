using System.Collections.Generic;
using GraphProcessor;
using UnityEngine;

namespace OrionReed
{
  [System.Serializable, NodeMenuItem("Entities/Random Size")]
  public class RandomEntitySampler : BaseEntityGraphNode, IEntitySettingsSampler
  {
    new public Color color = new Color(0.75f, 0.75f, 0.75f, 1f);
    public float min = 0.2f;
    public float max = 1.5f;

    [Output(name = "Out")]
    public IEntitySettingsSampler output;

    public override string name => "Random Entities";
    public override string layoutStyle => "EntitySettingsStyle";

    public IEntitySettings Get()
    {
      return new SimpleEntitySettings(color, Random.Range(min, max));
    }

    protected override void Process()
    {
      output = this;
    }
  }
}