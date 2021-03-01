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
    [NonSerialized] private EntityGraphProjector currentProjector;

    public static bool debugDrawBounds = true;
    public static bool debugDrawChunks = true;
    public static bool debugDrawEntities = true;
    public static bool debugDrawMaps = true;
    public static Color debugMapColor = Color.blue;
    public static float debugGizmoBrightness = 0.5f;

    public static List<EntityGraphProjector> ProjectorsInScene = new List<EntityGraphProjector>();

    public void SetCurrentRegion(Region region) => currentRegion = region;
    public Region GetCurrentRegion() => currentRegion;
    public void SetCurrentProjector(EntityGraphProjector projector) => currentProjector = projector;
    public EntityGraphProjector GetCurrentProjector() => currentProjector;

    public CoordinateRNG ChunkRandoms => rnd ??= new CoordinateRNG(seed);
    public Map Map
    {
      get { return map; }
      set { map = value; }
    }

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

    public void ResetRNG()
    {
      //map = null;
      rnd = null;
    }
  }
}