using System;
using GraphProcessor;
using UnityEngine;

namespace OrionReed
{
  [System.Serializable, NodeMenuItem("Entities/Simple")]
  public class SimpleEntitySampler : BaseEntityGraphNode, IEntitySampler
  {
    public override string name => "Simple Entities";

    new public Color color = new Color(0.75f, 0.75f, 0.75f, 1f);
    public float size = 0.5f;

    [Output(name = "Out")]
    public IEntitySampler output;


    public IEntitySettingsData Get => new SimpleEntitySettings(color, size);
    public IEntitySettingsData GetWithRandom(System.Random _) => Get;

    protected override void Process()
    {
      output = this;
    }
  }

  [Serializable]
  public class SimpleEntitySettings : IEntitySettingsData
  {
    public SimpleEntitySettings(Color color, float size)
    {
      Color = color;
      Size = size;
    }

    public Color Color { get; }
    public float Size { get; }
  }
}