using System.Collections.ObjectModel;
using UnityEngine;
using GraphProcessor;
using System;

namespace OrionReed
{
  [Serializable]
  public class EntityGraph : BaseGraph
  {
    public int seed = 1;

    #region ShouldNotBeHere
    public static ObservableCollection<EntityGraphProjector> ProjectorsInScene = new ObservableCollection<EntityGraphProjector>();

    [NonSerialized] private CoordinateRNG rnd;
    [NonSerialized] private Region currentRegion;
    [NonSerialized] private EntityGraphProjector currentProjector;
    [NonSerialized] private EntityCollection currentEntities = new EntityCollection();
    public void SetCurrentRegion(Region region) => currentRegion = region;
    public Region GetCurrentRegion() => currentRegion;
    public void SetCurrentProjector(EntityGraphProjector projector) => currentProjector = projector;
    public EntityGraphProjector GetCurrentProjector() => currentProjector;
    public CoordinateRNG ChunkRandoms => rnd ??= new CoordinateRNG(seed);
    #endregion

    public EntityGraph()
    {
      base.onEnabled += Initialize;
    }

    public void AddProcessedResult(EntityCollection entities)
    {
      currentEntities = entities;
      GetCurrentProjector().SetVisualization(currentEntities);
    }

    public void ProjectCurrent()
    {
      GetCurrentProjector().Project(currentEntities);
    }

    private void Initialize()
    {
      if (nodes.Find(n => n is OutputNode) == null)
      {
        AddNode(BaseNode.CreateFromType<OutputNode>(Vector2.one * 100));
      }
    }

    public void ResetRNG()
    {
      rnd = null;
    }
  }
}