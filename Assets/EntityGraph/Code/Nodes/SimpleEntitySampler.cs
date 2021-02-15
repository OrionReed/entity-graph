using System.Collections.Generic;
using GraphProcessor;
using UnityEngine;

namespace OrionReed
{
  [System.Serializable, NodeMenuItem("Entities/Simple")]
  public class SimpleEntitySampler : BaseEntityGraphNode, IEntitySettingsSampler
  {
    new public Color color = new Color(0.75f, 0.75f, 0.75f, 1f);
    public float size = 0.5f;

    [Output(name = "Out")]
    public IEntitySettingsSampler output;

    public override string name => "Simple Entities";

    public IEntitySettings Get()
    {
      return new SimpleEntitySettings(color, size);
    }

    public override string layoutStyle => "EntitySettingsStyle";

    protected override void Process()
    {
      output = this;
    }
  }

  [System.Serializable]
  public class SimpleEntitySettings : IEntitySettings
  {
    public SimpleEntitySettings(Color color, float size)
    {
      Color = color;
      Size = size;
    }

    public Color Color { get; }
    public float Size { get; }

    public void Visualize(Vector3 position)
    {
      Gizmos.color = Color;
      Gizmos.DrawSphere(position, Size);
    }
  }
}