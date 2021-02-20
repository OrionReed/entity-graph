using UnityEngine;
using System.Collections.Generic;

namespace OrionReed
{
  public struct PointProperties
  {
    public Matrix4x4 matrix;
    public Vector4 color;

    public static int Size() => (sizeof(float) * 4 * 4) + (sizeof(float) * 4);
  }

  [ExecuteInEditMode]
  public class EntityGraphPreview : MonoBehaviour
  {
    public EntityGraph graph;
    public Material material;
    public Mesh mesh;
    public bool drawGizmos;
    public bool drawInstanced;

    private Bounds bounds;
    private ComputeBuffer meshPropertiesBuffer;
    private ComputeBuffer argsBuffer;
    private PointProperties[] properties;

    private void OnDrawGizmos()
    {
      if (drawGizmos)
        graph?.Visualize();
    }

    private void Setup()
    {
      bounds = graph.OutputMasterNode.bounds;
      properties = GetPointPropertiesFromGraph();
      InitializeBuffers(properties);
    }

    private void Update()
    {
      if (graph?.EntityCache == null || graph.EntityCache.EntityCount == 0)
        return;

      if (properties != null && properties.Length == graph.EntityCache.EntityCount && drawInstanced)
      {
        Graphics.DrawMeshInstancedIndirect(mesh, 0, material, bounds, argsBuffer);
      }
      else
      {
        Setup();
      }
    }

    private PointProperties[] GetPointPropertiesFromGraph()
    {
      List<PointProperties> p = new List<PointProperties>(graph.EntityCache.EntityCount);
      foreach (IEntity entity in graph.EntityCache.AllEntities)
      {
        p.Add(new PointProperties
        {
          matrix = Matrix4x4.TRS(entity.Position, Quaternion.identity, Vector3.one * entity.Settings.Size),
          color = entity.Settings.Color
        });
      }
      return p.ToArray();
    }

    private void InitializeBuffers(PointProperties[] properties)
    {
      uint[] args = new uint[5] { 0, 0, 0, 0, 0 };
      args[0] = (uint)mesh.GetIndexCount(0);
      args[1] = (uint)properties.Length;
      args[2] = (uint)mesh.GetIndexStart(0);
      args[3] = (uint)mesh.GetBaseVertex(0);
      argsBuffer = new ComputeBuffer(1, args.Length * sizeof(uint), ComputeBufferType.IndirectArguments);
      argsBuffer.SetData(args);

      meshPropertiesBuffer = new ComputeBuffer(properties.Length, PointProperties.Size());
      meshPropertiesBuffer.SetData(properties);
      material.SetBuffer("_Properties", meshPropertiesBuffer);
    }

    void OnDisable()
    {
      meshPropertiesBuffer?.Release();
      meshPropertiesBuffer = null;
      argsBuffer?.Release();
      argsBuffer = null;
    }
  }
}