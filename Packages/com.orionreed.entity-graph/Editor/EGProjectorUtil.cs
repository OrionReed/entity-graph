using UnityEditor;
using System;
using UnityEngine;

namespace OrionReed
{
  public static class EGProjectorUtil
  {
    private static Color colDirty = new Color(1f, 0.513f, 0.078f);
    public static Color ColDefault => Color.white * EGWindow.debugGizmoBrightness;
    public static Color ColDirty => colDirty * EGWindow.debugGizmoBrightness;

    public static void DrawBoundsWire(Color color, Bounds bounds)
    {
      Handles.color = color;
      Handles.DrawWireCube(bounds.center, bounds.size);
    }

    public static void DrawTop(Color color, Bounds bounds)
    {
      Handles.color = color;
      Vector3 right = Vector3.right * bounds.size.x;
      Vector3 forward = Vector3.forward * bounds.size.z;
      Handles.DrawAAPolyLine(
          bounds.max,
          bounds.max - forward,
          bounds.max - forward - right,
          bounds.max - right,
          bounds.max
      );
    }

    public static void GraphMissing(Color color, Bounds bounds)
    {
      bounds.size *= 0.95f;
      Handles.color = color;
      Vector3 right = Vector3.right * bounds.size.x;
      Vector3 up = Vector3.up * bounds.size.y;
      Vector3 forward = Vector3.forward * bounds.size.z;
      Handles.DrawDottedLines(
        new Vector3[24] {

          // Vertical lines
          bounds.min, bounds.min + up,
          bounds.max, bounds.max + -up,
          bounds.min + forward, bounds.max - right,
          bounds.min + right, bounds.max - forward,

          // top
          bounds.max, bounds.max - forward,
          bounds.max, bounds.max - right,
          bounds.min + up, bounds.max - forward,
          bounds.min + up, bounds.max - right,

          // bottom
          bounds.min, bounds.min + forward,
          bounds.min, bounds.min + right,
          bounds.max - up, bounds.min + forward,
          bounds.max - up, bounds.min + right,
        },
        5f
      );
    }

    public static void DrawChunks(Color color, Bounds bounds)
    {
      Coordinate start = Coordinate.FromWorldSpace(bounds.center - bounds.extents);
      Coordinate end = Coordinate.FromWorldSpace(bounds.center + bounds.extents);
      int rowCount = Mathf.Abs(start.Y - end.Y) + 2;
      int columnCount = Mathf.Abs(start.X - end.X) + 2;
      float rowLength = Coordinate.scale * (columnCount - 1);
      float columnLength = Coordinate.scale * (rowCount - 1);

      Handles.color = color;

      Vector3 origin = Coordinate.FloorVector3(start);
      origin.y = bounds.center.y + bounds.extents.y;

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

    public static void DrawClippedChunks(Color color, Bounds bounds)
    {
      Coordinate start = Coordinate.FromWorldSpace(bounds.center - bounds.extents);
      Coordinate end = Coordinate.FromWorldSpace(bounds.center + bounds.extents);
      int rowCount = Mathf.Abs(start.Y - end.Y);
      int columnCount = Mathf.Abs(start.X - end.X);

      Handles.color = color;

      Vector3 origin = Coordinate.FloorVector3(start);
      origin.y = bounds.center.y + bounds.extents.y;

      for (int row = 1; row < rowCount + 1; row++)
      {
        Vector3 lineStart = new Vector3(bounds.center.x - bounds.extents.x, origin.y, origin.z + (row * Coordinate.scale));
        Handles.DrawAAPolyLine(lineStart, lineStart + (Vector3.right * bounds.size.x));
      }

      for (int column = 1; column < columnCount + 1; column++)
      {
        Vector3 lineStart = new Vector3(origin.x + (column * Coordinate.scale), origin.y, bounds.center.z - bounds.extents.z);
        Handles.DrawAAPolyLine(lineStart, lineStart + (Vector3.forward * bounds.size.z));
      }
    }
  }
}