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
    [NonSerialized] private Map _map;
    [NonSerialized] private Region _completeRegion;

    public Region CompleteRegion => _completeRegion ??= new Region(OutputMasterNode.bounds);
    public EntityCollection EntityCache { get; set; }
    public CoordinateRNG ChunkRandoms => rnd ??= new CoordinateRNG(OutputMasterNode.seed);
    public Map Map
    {
      get { return _map; }
      set { _map = value; }
    }

    public OutputMasterNode OutputMasterNode => _outputMasterNode ??= nodes.Find(n => n is OutputMasterNode) as OutputMasterNode;

    public event Action onFinishedProcessing;
    public event Action onClear;

    public EntityGraph()
    {
      base.onEnabled += Initialize;
    }

    private void Initialize()
    {
      if (OutputMasterNode == null)
      {
        AddNode(BaseNode.CreateFromType<OutputMasterNode>(Vector2.one * 100));
      }
    }

    public void OnFinishedProcessing() => onFinishedProcessing?.Invoke();

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

    public void Clear()
    {
      _map = null;
      _completeRegion = null;
      rnd = null;
      EntityCache = null;
      onClear?.Invoke();
    }
  }
}