using UnityEngine;
using System;

namespace OrionReed
{
  public interface IEntity
  {
    Vector2 Position { get; }
    string ID { get; }
    IEntityVisualizable Visualization { get; set; }
    IEntityInstantiatable Instantiation { get; set; }
  }

  public interface IEntityVisualizable
  {
    Color Color { get; }
    float Size { get; }
  }

  public interface IEntityInstantiatable
  {
    bool Instantiate(Vector3 position);
  }

  public interface IPositionSampler
  {
    float SamplePosition(Vector3 position);
  }
}