using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEditor;

namespace OrionReed
{
  [ExecuteAlways]
  public class EntityGraphProjector : MonoBehaviour
  {
    [SerializeField] private EntityGraph graph;
    [SerializeField] private Vector3 size = new Vector3(20, 5, 20);

    private Color chunkColorMasked = new Color(0.6f, 0.6f, 0.35f, 1);
    private Color chunkColorBoundless = Color.cyan;

    private EGPointRenderer vis;

    public EntityGraph Graph => graph;

    private void OnEnable() => EntityGraph.ProjectorsInScene.Add(this);

    private void OnDisable() => EntityGraph.ProjectorsInScene.Remove(this);

    private void Update() => vis?.Update();

    public Bounds GetBounds() => new Bounds(transform.position, size);

    public float ProjectionHeight() => transform.position.y + (size.y / 2f);

    public void ResetVisualiser(EntityCollection entities)
    {
      vis = new EGPointRenderer(GetBounds(), entities, ProjectionHeight(), transform.position);
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
      Handles.zTest = CompareFunction.LessEqual;
      Handles.color = Color.white * EntityGraph.debugGizmoBrightness;
      Handles.DrawWireCube(transform.position, size);
    }

    private void DrawChunks()
    {
      Coordinate start = Coordinate.FromWorldSpace(transform.position - (size / 2));
      Coordinate end = Coordinate.FromWorldSpace(transform.position + (size / 2));
      int rowCount = Mathf.Abs(start.Y - end.Y) + 2;
      int columnCount = Mathf.Abs(start.X - end.X) + 2;
      float rowLength = Coordinate.scale * (columnCount - 1);
      float columnLength = Coordinate.scale * (rowCount - 1);

      Handles.zTest = CompareFunction.LessEqual;
      Handles.color = chunkColorBoundless * EntityGraph.debugGizmoBrightness;

      Vector3 origin = Coordinate.WorldPositionZeroed(start);
      origin.y = transform.position.y + (size.y / 2f);

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
      Coordinate start = Coordinate.FromWorldSpace(transform.position - (size / 2));
      Coordinate end = Coordinate.FromWorldSpace(transform.position + (size / 2));
      int rowCount = Mathf.Abs(start.Y - end.Y);
      int columnCount = Mathf.Abs(start.X - end.X);

      Handles.zTest = CompareFunction.LessEqual;
      Handles.color = chunkColorMasked * EntityGraph.debugGizmoBrightness;

      Vector3 origin = Coordinate.WorldPositionZeroed(start);
      origin.y = ProjectionHeight();

      for (int row = 1; row < rowCount + 1; row++)
      {
        Vector3 lineStart = new Vector3(transform.position.x - (size.x / 2), origin.y, origin.z + (row * Coordinate.scale));
        Handles.DrawAAPolyLine(lineStart, lineStart + (Vector3.right * size.x));
      }

      for (int column = 1; column < columnCount + 1; column++)
      {
        Vector3 lineStart = new Vector3(origin.x + (column * Coordinate.scale), origin.y, transform.position.z - (size.z / 2));
        Handles.DrawAAPolyLine(lineStart, lineStart + (Vector3.forward * size.z));
      }
    }
  }
}