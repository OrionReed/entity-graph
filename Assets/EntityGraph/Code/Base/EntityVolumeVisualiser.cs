using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using System.Collections.Generic;
using System;

namespace OrionReed
{
  public class EntityVolumeVisualiser
  {

    private static readonly Material entityMat;
    private static readonly Material chunkMat;
    private static readonly Mesh entityMesh;
    private static readonly Mesh chunkMesh;
    private static Color chunkToProcess = new Color(1f, 0.533f, 0.219f);
    private static Color chunkProcessed = new Color(0.082f, 0.698f, 0.164f);
    private const float chunkMeshScale = 10;

    private EntityGraph graph;

    private ComputeBuffer entityMeshPropertiesBuffer;
    private ComputeBuffer entityArgsBuffer;
    private EntityValue[] entityValues;

    private Matrix4x4[] chunkMatrices;
    private MaterialPropertyBlock[] chunkProperties;

    private Bounds drawBounds;

    private EntityCollection entitiesToDraw;

    static EntityVolumeVisualiser()
    {
      entityMesh = EGVisualize.CreatePrimitiveMesh(PrimitiveType.Sphere);
      chunkMesh = EGVisualize.CreatePrimitiveMesh(PrimitiveType.Plane);
      entityMat = (Material)Resources.Load("ENTITY_POINT_MATERIAL", typeof(Material));
      chunkMat = (Material)Resources.Load("CHUNK_MATERIAL", typeof(Material));
    }

    public EntityVolumeVisualiser(EntityGraph g, Bounds renderBounds, EntityCollection entities)
    {
      Debug.Log("creating viz...");
      entitiesToDraw = entities;
      drawBounds = renderBounds;
      graph = g;
      graph.onFinishedProcessing += SetupEntities;
      graph.onFinishedProcessing += TrySetupMaps;
      //graph.onClear += ReleaseBuffers;
      AssemblyReloadEvents.beforeAssemblyReload += ReleaseBuffers;
    }

    public void Update()
    {
      if (graph == null) return;
      if (EntityGraph.debugDrawEntities) DrawEntities();
      if (EntityGraph.debugDrawMaps) DrawMaps();
    }

    void DrawEntities()
    {
      if (entitiesToDraw == null || entitiesToDraw.EntityCount == 0 || entityValues == null)
        return;

      Graphics.DrawMeshInstancedIndirect(entityMesh, 0, entityMat, drawBounds, entityArgsBuffer);
    }

    private void DrawMaps()
    {
      if (graph.Map == null) return;

      for (int i = 0; i < chunkMatrices.Length; i++)
      {
        Graphics.DrawMesh(chunkMesh, chunkMatrices[i], chunkMat, 0, null, 0, chunkProperties[i]);
      }
    }

    private void GetChunkValues()
    {
      Color col = new Color(0, 0, 0, 1);
      chunkMatrices = new Matrix4x4[graph.Map.ChunkCount()];
      chunkProperties = new MaterialPropertyBlock[graph.Map.ChunkCount()];
      Vector3 meshOffset = new Vector3(chunkMeshScale / 2, 0, chunkMeshScale / 2);
      int textureID = Shader.PropertyToID("_ChunkTex");
      int colorID = Shader.PropertyToID("_MapColor");
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
        chunkMatrices[index] = Matrix4x4.TRS(Coordinate.WorldPositionZeroed(c.Key) + meshOffset, Quaternion.Euler(0, 180, 0), Vector3.one);
        chunkProperties[index] = new MaterialPropertyBlock();
        chunkProperties[index].SetTexture(textureID, texture);
        chunkProperties[index].SetColor(colorID, EntityGraph.debugMapColor);
        chunkProperties[index].SetTexture(textureID, texture);
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

    void ReleaseBuffers()
    {
      entityMeshPropertiesBuffer?.Release();
      entityMeshPropertiesBuffer = null;
      entityArgsBuffer?.Release();
      entityArgsBuffer = null;
    }

    private EntityValue[] GetEntityValues()
    {
      List<EntityValue> p = new List<EntityValue>(entitiesToDraw.EntityCount);
      foreach (IEntity entity in entitiesToDraw.AllEntities)
      {
        p.Add(new EntityValue
        {
          matrix = Matrix4x4.TRS(new Vector3(entity.Position.x, 0, entity.Position.y) - drawBounds.center, Quaternion.identity, Vector3.one * entity.Settings.Size),
          color = entity.Settings.Color
        });
      }
      return p.ToArray();
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



    private struct EntityValue
    {
      public Matrix4x4 matrix;
      public Vector4 color;

      public static int Size() => (sizeof(float) * 4 * 4) + (sizeof(float) * 4);
    }
  }
}