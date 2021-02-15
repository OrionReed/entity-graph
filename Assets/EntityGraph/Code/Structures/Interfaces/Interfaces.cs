using UnityEngine;

namespace OrionReed
{
  public interface IEntity
  {
    Vector3 Position { get; }
    string ID { get; }
    IEntitySettings Settings { get; }
  }

  public interface IEntitySettings
  {
    public Color Color { get; }
    public float Size { get; }
    void Visualize(Vector3 position);
  }

  public interface IEntitySettingsSampler
  {
    IEntitySettings Get();
  }

  public interface ISampler
  {
    float SamplePosition(Vector3 position);
  }
}