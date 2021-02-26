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

    private Color chunkColor = new Color(0.5f, 0.5f, 0.25f, 1);

    private EntityVolumeVisualiser vis;

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
      vis = new EntityVolumeVisualiser(graph, GetBounds());
    }

    private void OnDrawGizmos()
    {
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

      Handles.color = chunkColor * EntityGraph.debugGizmoBrightness;

      Vector3 origin = Coordinate.WorldPosition(start);
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
  }
}