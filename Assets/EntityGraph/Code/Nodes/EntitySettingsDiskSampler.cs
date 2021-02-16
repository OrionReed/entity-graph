using System;
using System.Collections.Generic;
using GraphProcessor;
using UnityEngine;

namespace OrionReed
{
  [System.Serializable, NodeMenuItem("Entities/Disk")]
  public class EntitySettingsDiskSampler : BaseEntityGraphNode, IEntitySampler
  {
    new public Color color = new Color(0.75f, 0.75f, 0.75f, 1f);
    public float radius = 0.5f;

    [Output(name = "Out")]
    public IEntitySampler output;

    public override string name => "Disk Entities";

    public IEntitySettingsData Get => new DiskEntitySettings(color, radius);
    public IEntitySettingsData GetWithRandom(System.Random coordinate) => Get;

    protected override void Process()
    {
      output = this;
    }

  }

  [System.Serializable]
  public class DiskEntitySettings : IEntitySettingsData
  {
    public DiskEntitySettings(Color color, float radius)
    {
      Color = color;
      Size = radius;
    }

    public Color Color { get; }
    public float Size { get; }

    public void Visualize(Vector3 position)
    {
      UnityEditor.Handles.color = Color;
      UnityEditor.Handles.DrawSolidDisc(position, Vector3.up, Size);
    }
  }
}