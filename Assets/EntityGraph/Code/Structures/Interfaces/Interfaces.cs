using UnityEngine;
using System;

namespace OrionReed
{
  public interface IEntity
  {
    Vector3 Position { get; }
    string ID { get; }
    IEntitySettingsData Settings { get; }
  }

  public interface IEntitySettingsData
  {
    public Color Color { get; }
    public float Size { get; }
    void Visualize(Vector3 position);
  }

  public interface IEntitySampler
  {
    IEntitySettingsData Get { get; }

    IEntitySettingsData GetWithRandom(System.Random random);
  }

  public interface IPositionSampler
  {
    float SamplePosition(Vector3 position);
  }
}