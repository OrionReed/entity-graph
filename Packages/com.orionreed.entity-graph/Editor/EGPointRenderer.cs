using UnityEditor;
using UnityEngine;

namespace OrionReed
{
  public class EGPointRenderer
  {
    private static readonly Mesh entityMesh;

    private readonly Material entityMat;
    private ComputeBuffer entityMeshPropertiesBuffer;
    private ComputeBuffer entityArgsBuffer;

    private Bounds drawBounds;

    static EGPointRenderer()
    {
      entityMesh = CreatePrimitiveMesh(PrimitiveType.Sphere);
    }

    public EGPointRenderer(Bounds renderBounds, EntityCollection entities, float height, Vector3 position)
    {
      entityMat = (Material)Resources.Load("ENTITY_POINT_MATERIAL", typeof(Material));
      drawBounds = renderBounds;
      InitializePoints(PointsFromEntityCollection(entities, height, position));
      AssemblyReloadEvents.beforeAssemblyReload += ReleaseBuffers;
    }

    public void Update()
    {
      //if (!EGWindow.debugDrawEntities) return;
      //Graphics.DrawMeshInstancedIndirect(entityMesh, 0, entityMat, drawBounds, entityArgsBuffer);
    }

    void ReleaseBuffers()
    {
      entityMeshPropertiesBuffer?.Release();
      entityMeshPropertiesBuffer = null;
      entityArgsBuffer?.Release();
      entityArgsBuffer = null;
    }

    private ColoredPoint[] PointsFromEntityCollection(EntityCollection entities, float height, Vector3 pos)
    {
      if (entities.EntityCount == 0)
        Debug.LogWarning("Trying to draw empty entity collection...");

      ColoredPoint[] points = new ColoredPoint[entities.EntityCount];
      int index = 0;
      foreach (IEntity entity in entities.AllEntities)
      {
        points[index].matrix = Matrix4x4.TRS(new Vector3(entity.Position.x - pos.x, height - pos.y, entity.Position.y - pos.z), Quaternion.identity, Vector3.one * entity.Visualization.Size);
        //Debug.Log($"POINT {points[index].matrix}");
        points[index].color = entity.Visualization.Color;
        index++;
      }
      return points;
    }

    private void InitializePoints(ColoredPoint[] points)
    {
      uint[] args = new uint[5] { 0, 0, 0, 0, 0 };
      args[0] = (uint)entityMesh.GetIndexCount(0);
      args[1] = (uint)points.Length;
      args[2] = (uint)entityMesh.GetIndexStart(0);
      args[3] = (uint)entityMesh.GetBaseVertex(0);
      entityArgsBuffer = new ComputeBuffer(1, args.Length * sizeof(uint), ComputeBufferType.IndirectArguments);
      entityArgsBuffer.SetData(args);

      entityMeshPropertiesBuffer = new ComputeBuffer(points.Length, ColoredPoint.Size());
      entityMeshPropertiesBuffer.SetData(points);
      entityMat.SetBuffer("_Properties", entityMeshPropertiesBuffer);
    }

    private struct ColoredPoint
    {
      public Matrix4x4 matrix;
      public Vector4 color;

      public static int Size() => (sizeof(float) * 4 * 4) + (sizeof(float) * 4);
    }

    private static Mesh CreatePrimitiveMesh(PrimitiveType type)
    {
      GameObject gameObject = GameObject.CreatePrimitive(type);
      Mesh mesh = gameObject.GetComponent<MeshFilter>().sharedMesh;
      SceneView.DestroyImmediate(gameObject);
      return mesh;
    }
  }
}