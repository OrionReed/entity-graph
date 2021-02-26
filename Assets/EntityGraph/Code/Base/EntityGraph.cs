using UnityEngine;
using GraphProcessor;
using System;
using System.Collections.Generic;

namespace OrionReed
{
  [Serializable]
  public class EntityGraph : BaseGraph
  {
    public int seed = 1;
    [NonSerialized] private CoordinateRNG rnd;
    [NonSerialized] private Map map;
    [NonSerialized] private Region currentRegion;

    public static bool debugDrawBounds = true;
    public static bool debugDrawChunks = true;
    public static bool debugDrawEntities = true;
    public static bool debugDrawMaps = true;
    public static Color debugMapColor = Color.blue;
    public static float debugGizmoBrightness = 0.5f;

    public static List<EntityGraphVolume> VolumesInScene = new List<EntityGraphVolume>();

    public void SetCurrentRegion(Region region) => currentRegion = region;
    public Region GetCurrentRegion() => currentRegion;

    public CoordinateRNG ChunkRandoms => rnd ??= new CoordinateRNG(seed);
    public Map Map
    {
      get { return map; }
      set { map = value; }
    }

    public event Action onFinishedProcessing;

    public EntityGraph()
    {
      base.onEnabled += Initialize;
    }

    private void Initialize()
    {
      if (nodes.Find(n => n is OutputMasterNode) == null)
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
      map = null;
      rnd = null;
    }
  }
}