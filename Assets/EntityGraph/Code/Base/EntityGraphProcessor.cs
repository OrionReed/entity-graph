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
      List<Bounds> volumesToProcess = new List<Bounds>();
      foreach (EntityGraphVolume volume in EntityGraph.VolumesInScene)
      {
        if (volume.Graph.name == graph.name)
        {
          UnityEngine.Debug.Log($"Region to process: {volume.name}");
          volume.UpdateVisualiser();
          volumesToProcess.Add(volume.GetBounds());
        }
      }

      Region completeRegion = new Region(volumesToProcess);
      ProcessRegion(completeRegion);
    }

    private void ProcessRegion(Region region)
    {
      Stopwatch st = new Stopwatch();
      st.Start();
      graph.Clear();
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