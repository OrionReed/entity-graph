using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace OrionReed
{
  [Serializable]
  public class EntityCollection
  {
    private readonly Dictionary<string, IEntity> _entities = new Dictionary<string, IEntity>();
    private readonly Dictionary<Coordinate, HashSet<string>> _chunks = new Dictionary<Coordinate, HashSet<string>>();

    public int EntityCount => _entities.Count;
    public int ChunkCount => _chunks.Count;

    public void AddEntity(IEntity entity)
    {
      CallCounter.Count(this);
      Coordinate coord = Coordinate.FromWorldSpace(entity.Position);
      if (TryGetEntity(entity.ID, out IEntity _))
      {
        Debug.LogWarning("Entity hash collision. Previous entity will not be replaced.");
      }
      else
      {
        _entities[entity.ID] = entity;
        GetChunk(coord).Add(entity.ID);
      }
    }

    public void RemoveEntity(string key)
    {
      CallCounter.Count(this);

      IEntity entity = _entities[key];
      if (entity == null) return;
      _chunks[Coordinate.FromWorldSpace(entity.Position)].Add(entity.ID);
      _entities.Remove(key);
    }

    public bool TryGetEntity(string key, out IEntity entity)
    {
      if (_entities.TryGetValue(key, out IEntity e))
      {
        entity = e;
        return true;
      }
      else
      {
        entity = null;
        return false;
      }
    }

    public HashSet<string> GetChunk(Coordinate coordinate)
    {
      if (_chunks.TryGetValue(coordinate, out HashSet<string> c))
      {
        return c;
      }
      else
      {
        _chunks[coordinate] = new HashSet<string>();
        return _chunks[coordinate];
      }
    }

    public void CreateEmptyChunk(Coordinate coordinate)
    {
      _chunks[coordinate] = new HashSet<string>();
    }

    public bool ChunkExists(Coordinate coordinate)
    {
      return _chunks.ContainsKey(coordinate);
    }

    public IEnumerable<KeyValuePair<Coordinate, HashSet<string>>> AllChunks
    {
      get
      {
        foreach (KeyValuePair<Coordinate, HashSet<string>> chunk in _chunks)
        {
          yield return chunk;
        }
      }
    }

    public IEnumerable<IEntity> AllEntities
    {
      get
      {
        foreach (IEntity entity in _entities.Values) yield return entity;
      }
    }

    public void AddEntity(IEnumerable<IEntity> entities)
    {
      foreach (IEntity entity in entities)
      {
        AddEntity(entity);
      }
    }

    public void RemoveEntity(List<string> keys)
    {
      for (int i = 0; i < keys.Count; i++)
      {
        RemoveEntity(keys[i]);
      }
    }

    public static EntityCollection Merge(List<EntityCollection> matrices)
    {
      EntityCollection result = new EntityCollection();
      for (int i = 0; i < matrices.Count; i++)
      {
        if (matrices[i].EntityCount == 0) continue;
        matrices[i]._entities.ToList().ForEach(x => result._entities[x.Key] = x.Value);
        matrices[i]._chunks.ToList().ForEach(x => result._chunks[x.Key] = x.Value);
      }
      return result;
    }
    public static EntityCollection MergeIntoFirst(params EntityCollection[] matrices) => Merge(new List<EntityCollection>(matrices));
    public static EntityCollection MergeIntoFirst(List<EntityCollection> matrices) => Merge(matrices);

    public static EntityCollection SubtractLayers(EntityCollection radiusSource, EntityCollection target, float radius)
    {
      static bool DistanceLessThan(Vector3 a, Vector3 b, float distance) => (a - b).sqrMagnitude < distance * distance;
      HashSet<string> idsToRemove = new HashSet<string>();
      foreach (var c in radiusSource.AllEntities)
      {
        foreach (Coordinate coord in Coordinate.IntersectsCircle(c.Position, radius))
        {
          if (!target.ChunkExists(coord) || !radiusSource.ChunkExists(coord))
            continue;

          foreach (string entityID in target.GetChunk(coord))
          {
            if (!target.TryGetEntity(entityID, out IEntity entity)) continue;
            if (DistanceLessThan(c.Position, entity.Position, radius))
              idsToRemove.Add(entity.ID);
          }
        }
      }
      foreach (string key in idsToRemove)
      {
        target.RemoveEntity(key);
      }
      return target;
    }
  }
}