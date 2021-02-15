using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System.Linq;

namespace OrionReed
{
  [Serializable]
  public class EntityCollection
  {
    private readonly Dictionary<string, IEntity> _entities = new Dictionary<string, IEntity>();
    private readonly Dictionary<Coordinate, HashSet<string>> _chunks = new Dictionary<Coordinate, HashSet<string>>();
    private static readonly Dictionary<Coordinate, Vector3> _worldSpaceCache = new Dictionary<Coordinate, Vector3>();

    public int EntityCount => _entities.Count;
    public int ChunkCount => _chunks.Count;

    public EntityCollection() { }

    public EntityCollection(Bounds bounds)
    {
      foreach (Coordinate coord in Coordinate.InsideBounds(bounds))
      {
        CreateEmptyChunk(coord);
      }
    }

    public void AddEntity(IEntity entity)
    {
      CallCounter.Count(this);
      Coordinate coord = Coordinate.FromWorldSpace(entity.Position);
      if (TryGetEntity(entity.ID, out IEntity e))
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

    public static Vector3 GetWorldSpace(Coordinate coordinate)
    {
      if (_worldSpaceCache.TryGetValue(coordinate, out Vector3 result))
      {
        return result;
      }
      else
      {
        return _worldSpaceCache[coordinate] = Coordinate.WorldPosition(coordinate);
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

    public IEnumerable<HashSet<string>> AllChunkStrings
    {
      get
      {
        foreach (HashSet<string> chunk in _chunks.Values)
        {
          yield return chunk;
        }
      }
    }

    public IEnumerable<Coordinate> AllChunkCoordinates
    {
      get
      {
        foreach (Coordinate chunk in _chunks.Keys)
        {
          yield return chunk;
        }
      }
    }

    public IEnumerable<KeyValuePair<Coordinate, HashSet<string>>> AllChunkPairs
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

    public Dictionary<Coordinate, Vector3> WorldSpaceCache => _worldSpaceCache;

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
    public static EntityCollection MergeIntoFirst(params EntityCollection[] matrices)
    {
      return Merge(new List<EntityCollection>(matrices));
    }
    public static EntityCollection MergeIntoFirst(List<EntityCollection> matrices)
    {
      return Merge(matrices);
    }

    public void RemoveInRadiusOf(Vector3 worldPosition, float radius)
    {
      foreach (Coordinate coord in Coordinate.IntersectsCircle(worldPosition, radius))
      {
        if (!_chunks.TryGetValue(coord, out HashSet<string> value))
          continue;

        List<string> idsToRemove = new List<string>();
        foreach (string entityID in value)
        {
          if (!TryGetEntity(entityID, out IEntity entity)) continue;
          if (DistanceLessThan(worldPosition, entity.Position, radius))
          {
            idsToRemove.Add(entity.ID);
          }
        }
        RemoveEntity(idsToRemove);
      }
    }

    private bool DistanceLessThan(Vector3 a, Vector3 b, float distance) => (a - b).sqrMagnitude < distance * distance;

    private IEntity GetEntity(string key)
    {
      if (_entities.TryGetValue(key, out IEntity value))
      {
        return value;
      }
      else
      {
        Debug.LogError($"Entity {key} not found!");
        return null;
      }
    }
  }
}