using UnityEngine;

namespace OrionReed
{
  [System.Serializable]
  public class Entity : IEntity
  {
    public Entity(Vector3 pos, IEntitySettingsData settings)
    {
      ID = XXHash.GetHash(pos.x, pos.y).ToString();
      Position = pos;
      Settings = settings;
    }
    public Entity(Vector3 pos)
    {
      Position = pos;
      ID = XXHash.GetHash(pos.x, pos.y).ToString();
      Settings = null;
    }

    public string ID { get; }
    public Vector3 Position { get; }
    public IEntitySettingsData Settings { get; }
  }
}