using UnityEngine;
using System.Collections;
using Unity.Jobs;
using Unity.Collections;
using Unity.Rendering;
using Unity.Mathematics;

public class InstancedDrawingTest : MonoBehaviour
{
  [SerializeField]
  private Material material;

  [SerializeField]
  private float minScale = 0.15f;

  [SerializeField]
  private float maxScale = 0.2f;

  [SerializeField]
  private int count;

  [SerializeField]
  private float xBounds = 10;

  [SerializeField]
  private float yBounds = 10;

  private Mesh mesh;

  // Matrix here is a compressed transform information
  // xy is the position, z is rotation, w is the scale
  private ComputeBuffer transformBuffer;

  // uvBuffer contains float4 values in which xy is the uv dimension and zw is the texture offset
  private ComputeBuffer uvBuffer;
  private ComputeBuffer colorBuffer;

  // Save values hopefully...

  private uint[] args;

  private ComputeBuffer argsBuffer;

  private void Awake()
  {
    QualitySettings.vSyncCount = 0;
    Application.targetFrameRate = -1;

    this.mesh = CreateQuad();

    float4[] transforms = new float4[this.count];
    float4[] uvs = new float4[this.count];
    float4[] colors = new float4[this.count];

    const float maxRotation = Mathf.PI * 2;
    for (int i = 0; i < this.count; ++i)
    {
      // transform
      float x = UnityEngine.Random.Range(-xBounds, yBounds);
      float y = UnityEngine.Random.Range(-yBounds, yBounds);
      float rotation = UnityEngine.Random.Range(0, maxRotation);
      float scale = UnityEngine.Random.Range(this.minScale, this.maxScale);
      transforms[i] = new float4(x, y, rotation, scale);

      // UV
      float u = UnityEngine.Random.Range(0, 4) * 0.25f;
      float v = UnityEngine.Random.Range(0, 4) * 0.25f;
      uvs[i] = new float4(0.25f, 0.25f, u, v);

      // color
      float r = UnityEngine.Random.Range(0f, 1.0f);
      float g = UnityEngine.Random.Range(0f, 1.0f);
      float b = UnityEngine.Random.Range(0f, 1.0f);
      colors[i] = new float4(r, g, b, 1.0f);
    }

    this.transformBuffer = new ComputeBuffer(this.count, 16);
    this.transformBuffer.SetData(transforms);
    int matrixBufferId = Shader.PropertyToID("transformBuffer");
    this.material.SetBuffer(matrixBufferId, this.transformBuffer);

    this.uvBuffer = new ComputeBuffer(this.count, 16);
    this.uvBuffer.SetData(uvs);
    int uvBufferId = Shader.PropertyToID("uvBuffer");
    this.material.SetBuffer(uvBufferId, this.uvBuffer);

    this.colorBuffer = new ComputeBuffer(this.count, 16);
    this.colorBuffer.SetData(colors);
    int colorsBufferId = Shader.PropertyToID("colorsBuffer");
    this.material.SetBuffer(colorsBufferId, this.colorBuffer);

    this.args = new uint[] {
            6, (uint)this.count, 0, 0, 0
        };
    this.argsBuffer = new ComputeBuffer(1, this.args.Length * sizeof(uint), ComputeBufferType.IndirectArguments);
    this.argsBuffer.SetData(this.args);
  }

  private static readonly Bounds BOUNDS = new Bounds(Vector2.zero, Vector3.one);

  private void Update() => Graphics.DrawMeshInstancedIndirect(this.mesh, 0, this.material, BOUNDS, this.argsBuffer);

  private static Mesh CreateQuad()
  {
    Mesh mesh = new Mesh();
    Vector3[] vertices = new Vector3[4];
    vertices[0] = new Vector3(0, 0, 0);
    vertices[1] = new Vector3(1, 0, 0);
    vertices[2] = new Vector3(0, 1, 0);
    vertices[3] = new Vector3(1, 1, 0);
    mesh.vertices = vertices;

    int[] tri = new int[6];
    tri[0] = 0;
    tri[1] = 2;
    tri[2] = 1;
    tri[3] = 2;
    tri[4] = 3;
    tri[5] = 1;
    mesh.triangles = tri;

    Vector3[] normals = new Vector3[4];
    normals[0] = -Vector3.forward;
    normals[1] = -Vector3.forward;
    normals[2] = -Vector3.forward;
    normals[3] = -Vector3.forward;
    mesh.normals = normals;

    Vector2[] uv = new Vector2[4];
    uv[0] = new Vector2(0, 0);
    uv[1] = new Vector2(1, 0);
    uv[2] = new Vector2(0, 1);
    uv[3] = new Vector2(1, 1);
    mesh.uv = uv;

    return mesh;
  }
}