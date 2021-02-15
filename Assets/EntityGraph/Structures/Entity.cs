using UnityEngine;

namespace OrionReed
{
  [System.Serializable]
  public class Entity : IEntity
  {
    public Entity(Vector3 pos, IEntitySettings settings)
    {
      ID = XXHash.GetHash(pos.x, pos.y).ToString();
      Position = pos;
      Settings = settings;
    }

    public string ID { get; }
    public Vector3 Position { get; }
    public IEntitySettings Settings { get; }
  }
}