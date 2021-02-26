using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace OrionReed
{
  [ExecuteAlways]
  public class EntityGraphVolume : MonoBehaviour
  {
    [SerializeField] private EntityGraph graph;
    [SerializeField] public Vector3 bounds = new Vector3(20, 5, 20);

    private Color chunkColorMasked = new Color(0.6f, 0.6f, 0.35f, 1);
    private Color chunkColorBoundless = Color.cyan;

    private EntityVolumeVisualiser vis;
    private EntityCollection processedCollection = new EntityCollection();

    public EntityGraph Graph => graph;

    private void OnEnable()
    {
      EntityGraph.VolumesInScene.Add(this);
    }

    private void OnDisable()
    {
      EntityGraph.VolumesInScene.Remove(this);
    }

    private void Update()
    {
      vis?.Update();
    }

    public Bounds GetBounds()
    {
      return new Bounds(transform.position, bounds);
    }

    public void UpdateVisualiser()
    {
      Debug.Log("updating vis...");
      vis = new EntityVolumeVisualiser(graph, GetBounds(), processedCollection);
    }

    private void OnDrawGizmos()
    {
      if (EntityGraph.debugDrawBounds && EntityGraph.debugDrawChunks)
      {
        DrawBounds();
        DrawClippedChunks();
        return;
      }
      if (EntityGraph.debugDrawBounds)
        DrawBounds();
      if (EntityGraph.debugDrawChunks)
        DrawChunks();
    }

    private void DrawBounds()
    {
      Handles.color = Color.white * EntityGraph.debugGizmoBrightness;
      Handles.DrawWireCube(transform.position, bounds);
    }

    private void DrawChunks()
    {
      Coordinate start = Coordinate.FromWorldSpace(transform.position - (bounds / 2));
      Coordinate end = Coordinate.FromWorldSpace(transform.position + (bounds / 2));
      int rowCount = Mathf.Abs(start.Y - end.Y) + 2;
      int columnCount = Mathf.Abs(start.X - end.X) + 2;
      float rowLength = Coordinate.scale * (columnCount - 1);
      float columnLength = Coordinate.scale * (rowCount - 1);

      Handles.color = chunkColorBoundless * EntityGraph.debugGizmoBrightness;

      Vector3 origin = Coordinate.WorldPositionZeroed(start);
      origin.y = transform.position.y + (bounds.y / 2f);

      for (int row = 0; row < rowCount; row++)
      {
        Vector3 linePos = origin + new Vector3(0, 0, row * Coordinate.scale);
        Handles.DrawAAPolyLine(linePos, linePos + (Vector3.right * rowLength));
      }

      for (int column = 0; column < columnCount; column++)
      {
        Vector3 linePos = origin + new Vector3(column * Coordinate.scale, 0, 0);
        Handles.DrawAAPolyLine(linePos, linePos + (Vector3.forward * columnLength));
      }
    }
    private void DrawClippedChunks()
    {
      Coordinate start = Coordinate.FromWorldSpace(transform.position - (bounds / 2));
      Coordinate end = Coordinate.FromWorldSpace(transform.position + (bounds / 2));
      int rowCount = Mathf.Abs(start.Y - end.Y);
      int columnCount = Mathf.Abs(start.X - end.X);

      Handles.color = chunkColorMasked * EntityGraph.debugGizmoBrightness;

      Vector3 origin = Coordinate.WorldPositionZeroed(start);
      origin.y = transform.position.y + (bounds.y / 2f);

      for (int row = 1; row < rowCount + 1; row++)
      {
        Vector3 lineStart = new Vector3(transform.position.x - (bounds.x / 2), origin.y, origin.z + (row * Coordinate.scale));
        Handles.DrawAAPolyLine(lineStart, lineStart + (Vector3.right * bounds.x));
      }

      for (int column = 1; column < columnCount + 1; column++)
      {
        Vector3 lineStart = new Vector3(origin.x + (column * Coordinate.scale), origin.y, transform.position.z - (bounds.z / 2));
        Handles.DrawAAPolyLine(lineStart, lineStart + (Vector3.forward * bounds.z));
      }
    }
  }
}