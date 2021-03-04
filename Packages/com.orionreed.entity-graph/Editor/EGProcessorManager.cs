using System.Collections.Generic;
using System;

namespace OrionReed
{
  public class EGProcessorManager
  {
    private Dictionary<EntityGraph, EntityGraphProcessor> processors = new Dictionary<EntityGraph, EntityGraphProcessor>();

    public void AddProcessor(EntityGraph graph)
    {
      if (!processors.ContainsKey(graph))
        processors[graph] = new EntityGraphProcessor(graph);
    }

    public void RemoveProcessor(EntityGraph graph)
    {
      if (processors.ContainsKey(graph))
        processors.Remove(graph);
    }

    public void ProcessRegion(EntityGraph graph, Region region)
    {
      if (processors.ContainsKey(graph))
        processors[graph].ProcessRegion(region);
    }

    public void ClearAllCaches()
    {
      // clear graph window state
      // clear projectors
      // clear processors
    }
  }
}