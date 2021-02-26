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
      foreach (EntityGraphVolume volume in EntityGraph.VolumesInScene)
      {
        if (volume.Graph.name == graph.name)
        {
          UnityEngine.Debug.Log($"Processing Volume: {volume.name}");
          volume.UpdateVisualiser();
          ProcessRegion(new Region(volume.GetBounds()));
        }
      }
    }

    private void ProcessRegion(Region region)
    {
      Stopwatch st = new Stopwatch();
      st.Start();
      //graph.Clear();
      graph.SetCurrentRegion(region);
      UpdateComputeOrder();
      Run();
      graph.OnFinishedProcessing();
      UnityEngine.Debug.LogWarning(st.Elapsed);
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