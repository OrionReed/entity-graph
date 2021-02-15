using UnityEngine;
using GraphProcessor;
using System;

namespace OrionReed
{
  // Needs investigation into why stuff cant be serialized. This is all clunky and will scale horribly.
  [System.Serializable]
  public class EntityGraph : BaseGraph
  {
    private ChunkRandoms rnd;
    private EntityChunkMatrix entityCache;
    private OutputMasterNode _outputMasterNode;

    public ChunkRandoms ChunkRandoms => rnd ??= new ChunkRandoms(OutputMasterNode.seed);
    public EntityChunkMatrix EntityCache
    {
      get
      {
        return entityCache ??= new EntityChunkMatrix();
      }
      set { entityCache = value; }
    }

    public OutputMasterNode OutputMasterNode
    {
      get => _outputMasterNode ??= nodes.Find(n => n is OutputMasterNode) as OutputMasterNode;
      internal set => _outputMasterNode = value;
    }

    public EntityGraph()
    {
      base.onEnabled += Initialize;
    }

    private void Initialize()
    {
      if (OutputMasterNode == null)
      {
        _outputMasterNode = AddNode(BaseNode.CreateFromType<OutputMasterNode>(Vector2.one * 100)) as OutputMasterNode;
      }
    }

    protected override void MigrateDeprecatedNodes()
    {
      for (int i = 0; i < nodes.Count; i++)
      {
        if (nodes[i] == null) continue;
        if (Attribute.GetCustomAttribute(nodes[i].GetType(), typeof(Deprecated)) is Deprecated deprecated)
        {
          Debug.LogWarning($"{nodes[i].GetType().Name} is deprecated. Upgrading to {deprecated.newType.Name}.");

          var json = JsonUtility.ToJson(nodes[i]);
          nodes[i] = JsonUtility.FromJson(json, deprecated.newType) as BaseNode;
        }
      }
    }

    public void Visualize()
    {
      OutputMasterNode.Visualize();
      if (EntityCache.ChunkCount > 0)
      {
        foreach (Coordinate chunk in EntityCache.AllChunkCoordinates)
        {
          Utils.DrawBounds(Coordinate.GetWorldPositionFromCoordinate(chunk), Vector3.one * EntityChunkMatrix.chunkSize, Color.cyan / 8);
        }
      }
      if (EntityCache.EntityCount > 0)
      {
        foreach (IEntity entity in EntityCache.AllEntities)
        {
          entity.Settings.Visualize(entity.Position);
        }
      }
    }

    public void Reset()
    {
      rnd = null;
      entityCache = null;
    }
  }
}