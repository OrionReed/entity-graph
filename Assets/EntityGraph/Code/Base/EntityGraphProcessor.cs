using System.Collections.Generic;
using System.Linq;
using GraphProcessor;
using Unity.Jobs;
using UnityEngine;

namespace OrionReed
{
  public class EntityGraphProcessor : BaseGraphProcessor
  {
    private List<BaseNode> processList = new List<BaseNode>();
    private EntityGraph EntityGraph => graph as EntityGraph;

    public EntityGraphProcessor(EntityGraph graph) : base(graph)
    {
    }

    public void ProcessEntityGraph()
    {
      EntityGraph.Reset();
      UpdateComputeOrder();
      Run();
      CallCounter.Results();
    }

    public override void UpdateComputeOrder()
    {
      processList = graph.nodes.OrderBy(n => n.computeOrder).ToList();
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