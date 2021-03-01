using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using System.Collections.Generic;
using System;

namespace OrionReed
{
  public class EGChunkRenderer
  {
    private static readonly Material chunkMat;
    private static readonly Mesh chunkMesh;
    private const float chunkMeshScale = 10;

    private Matrix4x4[] chunkMatrices;
    private MaterialPropertyBlock[] chunkProperties;

    private readonly Map map;

    static EGChunkRenderer()
    {
      chunkMesh = CreatePrimitiveMesh(PrimitiveType.Plane);
      chunkMat = (Material)Resources.Load("CHUNK_MATERIAL", typeof(Material));
    }

    public EGChunkRenderer(Map mapToDraw)
    {
      map = mapToDraw;
      if (mapToDraw == null)
      {
        Debug.LogError("Atempting to setup renderer for null map");
        return;
      }
      ProcessChunk();
    }

    public void Update()
    {
      if (EntityGraph.debugDrawMaps) DrawMaps();
    }

    private void DrawMaps()
    {
      if (map == null) return;

      for (int i = 0; i < chunkMatrices.Length; i++)
      {
        Graphics.DrawMesh(chunkMesh, chunkMatrices[i], chunkMat, 0, null, 0, chunkProperties[i]);
      }
    }

    private void ProcessChunk()
    {
      Color col = new Color(0, 0, 0, 1);
      chunkMatrices = new Matrix4x4[map.ChunkCount()];
      chunkProperties = new MaterialPropertyBlock[map.ChunkCount()];
      Vector3 meshOffset = new Vector3(chunkMeshScale / 2, 0, chunkMeshScale / 2);
      int textureID = Shader.PropertyToID("_ChunkTex");
      int colorID = Shader.PropertyToID("_MapColor");
      int index = 0;
      foreach (var c in map.IterateChunks())
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
        chunkMatrices[index] = Matrix4x4.TRS(Coordinate.FloorVector3(c.Key) + meshOffset, Quaternion.Euler(0, 180, 0), Vector3.one);
        chunkProperties[index] = new MaterialPropertyBlock();
        chunkProperties[index].SetTexture(textureID, texture);
        chunkProperties[index].SetColor(colorID, EntityGraph.debugMapColor);
        chunkProperties[index].SetTexture(textureID, texture);
        index++;
      }
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