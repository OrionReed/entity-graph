using UnityEngine;

namespace OrionReed
{
  [System.Serializable]
  public class Entity : IEntity
  {
    public Entity(Vector2 pos)
    {
      Position = pos;
      ID = XXHash.GetHash(pos.x, pos.y).ToString();
      Visualization = null;
      Instantiation = null;
    }

    public string ID { get; }
    public Vector2 Position { get; }
    public IEntityVisualizable Visualization { get; set; }
    public IEntityInstantiatable Instantiation { get; set; }
  }
}