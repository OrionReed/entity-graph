using UnityEngine;
using System.Collections.Generic;

namespace OrionReed
{
  public struct EntityValue
  {
    public Matrix4x4 matrix;
    public Vector4 color;

    public static int Size() => (sizeof(float) * 4 * 4) + (sizeof(float) * 4);
  }

  [ExecuteInEditMode]
  public class EntityGraphPreview : MonoBehaviour
  {
    public EntityGraph graph;
    public bool drawGizmos;
    public bool drawInstanced;
    [Space]
    public Material entityMat;
    public Mesh entityMesh;
    [Space]
    public Material chunkMat;
    public Mesh chunkMesh;

    private Bounds bounds;
    private ComputeBuffer entityMeshPropertiesBuffer;
    private ComputeBuffer entityArgsBuffer;
    private EntityValue[] entityValues;

    private Matrix4x4[] chunkMatrices;
    private MaterialPropertyBlock[] chunkProperties;
    private const float chunkMeshScale = 10;

    private void OnEnable()
    {
      graph.onFinishedProcessing += UpdateBounds;
      graph.onFinishedProcessing += SetupEntities;
      graph.onFinishedProcessing += TrySetupMaps;
    }

    private void OnDrawGizmos()
    {
      if (drawGizmos)
        graph?.Visualize();
    }

    private void UpdateBounds()
    {
      bounds = graph.OutputMasterNode.bounds;
    }

    private void SetupEntities()
    {
      entityValues = GetEntityValues();
      BufferEntitites(entityValues);
    }

    private void TrySetupMaps()
    {
      if (graph.Map == null)
        return;
      GetChunkValues();

    }

    private void Update()
    {
      if (!drawInstanced || graph?.EntityCache == null || graph.EntityCache.EntityCount == 0 || entityValues == null)
        return;

      Graphics.DrawMeshInstancedIndirect(entityMesh, 0, entityMat, bounds, entityArgsBuffer);

      if (graph.Map == null)
        return;
      for (int i = 0; i < chunkMatrices.Length; i++)
      {
        Graphics.DrawMesh(chunkMesh, chunkMatrices[i], chunkMat, 0, null, 0, chunkProperties[i]);
      }
    }

    private EntityValue[] GetEntityValues()
    {
      List<EntityValue> p = new List<EntityValue>(graph.EntityCache.EntityCount);
      foreach (IEntity entity in graph.EntityCache.AllEntities)
      {
        p.Add(new EntityValue
        {
          matrix = Matrix4x4.TRS(entity.Position, Quaternion.identity, Vector3.one * entity.Settings.Size),
          color = entity.Settings.Color
        });
      }
      return p.ToArray();
    }

    private void GetChunkValues()
    {
      Color col = new Color(0, 0, 0, 1);
      chunkMatrices = new Matrix4x4[graph.Map.ChunkCount()];
      chunkProperties = new MaterialPropertyBlock[graph.Map.ChunkCount()];
      Vector3 meshOffset = new Vector3(chunkMeshScale / 2, 0, chunkMeshScale / 2);
      int texID = Shader.PropertyToID("_ChunkTex");
      int index = 0;
      foreach (var c in graph.Map.IterateChunks())
      {

        Texture2D texture = new Texture2D(c.Value.GetLength(0), c.Value.GetLength(1), TextureFormat.Alpha8, false);
        for (int x = 0; x < c.Value.GetLength(0); x++)
        {
          for (int y = 0; y < c.Value.GetLength(1); y++)
          {
            texture.SetPixel(x, y, col * c.Value[x, y]);
          }
        }
        texture.Apply();
        chunkMatrices[index] = Matrix4x4.TRS(Coordinate.WorldPosition(c.Key) + meshOffset, Quaternion.Euler(0, 180, 0), Vector3.one);
        chunkProperties[index] = new MaterialPropertyBlock();
        chunkProperties[index].SetTexture(texID, texture);
        index++;
      }
    }

    private void BufferEntitites(EntityValue[] properties)
    {
      uint[] args = new uint[5] { 0, 0, 0, 0, 0 };
      args[0] = (uint)entityMesh.GetIndexCount(0);
      args[1] = (uint)properties.Length;
      args[2] = (uint)entityMesh.GetIndexStart(0);
      args[3] = (uint)entityMesh.GetBaseVertex(0);
      entityArgsBuffer = new ComputeBuffer(1, args.Length * sizeof(uint), ComputeBufferType.IndirectArguments);
      entityArgsBuffer.SetData(args);

      entityMeshPropertiesBuffer = new ComputeBuffer(properties.Length, EntityValue.Size());
      entityMeshPropertiesBuffer.SetData(properties);
      entityMat.SetBuffer("_Properties", entityMeshPropertiesBuffer);
    }

    void OnDisable()
    {
      entityMeshPropertiesBuffer?.Release();
      entityMeshPropertiesBuffer = null;
      entityArgsBuffer?.Release();
      entityArgsBuffer = null;
    }
  }
}