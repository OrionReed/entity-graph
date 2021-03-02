using GraphProcessor;
using UnityEngine;
using System.Collections.Generic;

namespace OrionReed
{
  [System.Serializable, NodeMenuItem("Entities/Fixed Size")]
  public class EntityFixedSize : BaseEntityNode
  {
    public override string name => "Fixed Size Entities";

    public float size = 1f;

    protected override void Process()
    {
      foreach (IEntity entity in input.AllEntities)
        entity.Visualization = new EntityVizFixedSizeRandomColor(size);

      output = input;
    }
  }

  public class EntityVizFixedSizeRandomColor : IEntityVisualizable
  {
    public EntityVizFixedSizeRandomColor(float size)
    {
      Size = size;
    }
    public Color Color => Random.ColorHSV();

    public float Size { get; }
  }
}