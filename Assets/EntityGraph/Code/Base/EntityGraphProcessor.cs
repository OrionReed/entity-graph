using System.Collections.Generic;
using System.Linq;
using GraphProcessor;
using Unity.Jobs;
using System.Diagnostics;
using UnityEngine;

namespace OrionReed
{
  public class EntityGraphProcessor : BaseGraphProcessor
  {
    new private readonly EntityGraph graph;
    private List<BaseNode> processList = new List<BaseNode>();

    public EntityGraphProcessor(EntityGraph graph) : base(graph)
    {
      this.graph = base.graph as EntityGraph;
    }

    public void ProcessAllInstancesInScene()
    {
      UpdateComputeOrder();
      foreach (EntityGraphProjector projector in EntityGraph.ProjectorsInScene)
      {
        if (projector.Graph.name == graph.name)
        {
          graph.ResetRNG();
          UnityEngine.Debug.Log($"Processing Volume: {projector.name}");
          graph.SetCurrentRegion(new Region(projector.GetBounds()));
          graph.SetCurrentProjector(projector);
          //Stopwatch st = new Stopwatch();
          //st.Start();
          Run();
        }
      }
    }

    public override void UpdateComputeOrder()
    {
      processList = base.graph.nodes.OrderBy(n => n.computeOrder).ToList();
    }

    public override void Run()
    {
      int count = processList.Count;

      for (int i = 0; i < count; i++)
      {
        processList[i].OnProcess();
      }

      JobHandle.ScheduleBatchedJobs();
    }
  }
}