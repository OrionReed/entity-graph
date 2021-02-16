using UnityEngine;
using GraphProcessor;
using System;

namespace OrionReed
{
  [Serializable]
  public class EntityGraph : BaseGraph
  {
    [NonSerialized] private CoordinateRNG rnd;
    [NonSerialized] private OutputMasterNode _outputMasterNode;
    [NonSerialized] private Region _completeRegion;

    public Region CompleteRegion => _completeRegion ??= new Region(OutputMasterNode.bounds);

    public EntityGraph()
    {
      base.onEnabled += Initialize;
    }

    public CoordinateRNG ChunkRandoms => rnd ??= new CoordinateRNG(OutputMasterNode.seed);
    public EntityCollection EntityCache { get; set; }

    private OutputMasterNode OutputMasterNode => _outputMasterNode ??= nodes.Find(n => n is OutputMasterNode) as OutputMasterNode;

    private void Initialize()
    {
      if (OutputMasterNode == null)
      {
        AddNode(BaseNode.CreateFromType<OutputMasterNode>(Vector2.one * 100));
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
      if (EntityCache?.ChunkCount > 0)
      {
        foreach (Coordinate chunk in CompleteRegion.EnumerateCoordinates())
        {
          Util.DrawBoundsFromCorners(Coordinate.WorldPosition(chunk), Vector3.one * Coordinate.scale, Color.cyan / 7);
        }
      }
      if (EntityCache?.EntityCount > 0)
      {
        foreach (IEntity entity in EntityCache.AllEntities)
        {
          entity.Settings.Visualize(entity.Position);
        }
      }
    }

    public void Reset()
    {
      _completeRegion = null;
      rnd = null;
      EntityCache = null;
    }
  }
}

