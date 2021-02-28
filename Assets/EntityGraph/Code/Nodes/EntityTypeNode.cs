using System.Collections.Generic;
using GraphProcessor;
using UnityEngine;
using OrionReed;

namespace OrionReed
{
  [System.Serializable, NodeMenuItem("Entitiy Type")]
  public class EntityTypeNode : BaseEntityGraphNode//, IEntitySampler
  {
    public EntityTypeOptions entityType = EntityTypeOptions.FixedScale;
    new public Color color = new Color(0.75f, 0.75f, 0.75f, 1f);
    [VisibleIf("entityType", EntityTypeOptions.FixedScale)]
    public float size = 5f;
    [VisibleIf("entityType", EntityTypeOptions.RangedScale)]
    public float min = 0.2f;
    [VisibleIf("entityType", EntityTypeOptions.RangedScale)]
    public float max = 1.5f;

    [Output(name = "Out")]
    public Foo<EntityTypeOptions> output;

    public override string name => "Entity Type";

    public IEntitySettingsData Get => new SimpleEntitySettings(color, max);

    public IEntitySettingsData GetWithRandom(System.Random rng)
    {
      return new SimpleEntitySettings(color, rng.NextFloat(min, max));
    }

    protected override void Process()
    {
      //output = this;
    }
  }

  public enum EntityTypeOptions
  {
    FixedScale,
    RangedScale,
    MapScale
  }

  [System.Serializable]
  public class Foo<T> where T : System.Enum
  {
    public Foo()
    {

    }
  }
}