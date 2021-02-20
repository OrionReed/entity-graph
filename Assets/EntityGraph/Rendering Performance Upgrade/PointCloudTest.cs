using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public struct PointProperties
{
  public Matrix4x4 matrix;
  public Vector4 color;

  public static int Size() => (sizeof(float) * 4 * 4) + (sizeof(float) * 4);
}

public struct Point
{
  public Vector3 position;
  public float size;
  public Color color;
}

[ExecuteInEditMode]
public class PointCloudTest : MonoBehaviour
{
  public int numberOfPoints = 50;
  public Material material;
  public Mesh mesh;
  public Bounds bounds;

  private ComputeBuffer meshPropertiesBuffer;
  private ComputeBuffer argsBuffer;
  PointProperties[] properties;

  [ContextMenu("Execute!")]
  private void Execute()
  {
    List<Point> points = GenerateExamplePoints(numberOfPoints);
    properties = ConvertPointsToProperties(points);
    InitializeBuffers(properties);
  }


  private void Update()
  {
    if (properties != null)
      Graphics.DrawMeshInstancedIndirect(mesh, 0, material, bounds, argsBuffer);
  }


  private List<Point> GenerateExamplePoints(int count)
  {
    List<Point> p = new List<Point>();
    for (int i = 0; i < count; i++)
    {
      p.Add(new Point()
      {
        position = new Vector3(Random.value * 100, Random.value * 100, Random.value * 100),
        size = Random.value,
        color = Random.ColorHSV()
      });
    }
    return p;
  }

  private PointProperties[] ConvertPointsToProperties(List<Point> points)
  {
    PointProperties[] p = new PointProperties[points.Count];
    for (int i = 0; i < points.Count; i++)
    {
      p[i].matrix = Matrix4x4.TRS(points[i].position, Quaternion.identity, Vector3.one * points[i].size);
      p[i].color = points[i].color;
    }
    return p;
  }

  private void InitializeBuffers(PointProperties[] properties)
  {
    uint[] args = new uint[5] { 0, 0, 0, 0, 0 };
    args[0] = (uint)mesh.GetIndexCount(0);
    args[1] = (uint)numberOfPoints;
    args[2] = (uint)mesh.GetIndexStart(0);
    args[3] = (uint)mesh.GetBaseVertex(0);
    argsBuffer = new ComputeBuffer(1, args.Length * sizeof(uint), ComputeBufferType.IndirectArguments);
    argsBuffer.SetData(args);

    meshPropertiesBuffer = new ComputeBuffer(numberOfPoints, PointProperties.Size());
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

