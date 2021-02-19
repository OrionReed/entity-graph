using System;
using UnityEngine;
using System.Collections.Generic;

namespace OrionReed
{
  [Serializable]
  public struct Coordinate
  {
    public static int scale = 10;

    public Coordinate(int x, int y)
    {
      X = x;
      Y = y;
    }

    public static Vector3 WorldPosition(Coordinate coordinate)
    {
      float x = coordinate.X * scale;
      float z = coordinate.Y * scale;
      return new Vector3(x, 0, z);
    }

    public static Coordinate FromWorldSpace(Vector3 position) => FromWorldSpace(position.x, position.z);
    public static Coordinate FromWorldSpace(int x, int y) => FromWorldSpace(x, y);

    public static Coordinate FromWorldSpace(float x, float y)
    {
      CallCounter.Count("Coordinate.FromWorldSpace");
      return new Coordinate(Mathf.FloorToInt(x / scale), Mathf.FloorToInt(y / scale));
    }

    public static Coordinate FromWorldSpace(Vector3 position, out Vector3 positionInsideChunk)
    {
      var result = FromWorldSpace(position);
      positionInsideChunk = position - WorldPosition(result);
      return result;
    }

    public static IEnumerable<Coordinate> IntersectsCircle(Vector3 position, float radius)
    {
      Coordinate centerCoord = FromWorldSpace(position);
      int max = (int)(Util.RoundDownToDivisor(scale + radius, scale) / scale);

      for (int x = centerCoord.X - max; x <= centerCoord.X + max; x++)
      {
        for (int y = centerCoord.Y - max; y <= centerCoord.Y + max; y++)
        {
          yield return new Coordinate(x, y);
        }
      }
    }
    public static IEnumerable<Coordinate> IterateRect(Coordinate start, Coordinate end)
    {
      for (int x = start.X; x < end.X; x++)
      {
        for (int y = start.Y; y < end.Y; y++)
        {
          yield return new Coordinate(x, y);
        }
      }
    }

    public static IEnumerable<Coordinate> InsideBounds(Bounds bounds)
    {
      int xOnBoundary = Mathf.Approximately(bounds.max.x % scale, 0) ? -1 : 0;
      int zOnBoundary = Mathf.Approximately(bounds.max.z % scale, 0) ? -1 : 0;

      Coordinate startCoord = FromWorldSpace(bounds.min);
      Coordinate endCoord = FromWorldSpace(bounds.max);

      for (int x = startCoord.X; x <= endCoord.X + xOnBoundary; x++)
      {
        for (int y = startCoord.Y; y <= endCoord.Y + zOnBoundary; y++)
        {
          yield return new Coordinate(x, y);
        }
      }
    }
    public static Bounds BoundsAroundEnclosedChunks(Bounds bounds)
    {
      int xOnBoundary = Mathf.Approximately(bounds.max.x % scale, 0) ? -1 : 0;
      int zOnBoundary = Mathf.Approximately(bounds.max.z % scale, 0) ? -1 : 0;

      Coordinate m = FromWorldSpace(bounds.max);
      int x = m.X + xOnBoundary;
      int y = m.Y + zOnBoundary;
      return new Bounds
      {
        min = WorldPosition(FromWorldSpace(bounds.min)),
        max = WorldPosition(new Coordinate(x, y)) + (Vector3.one * scale)
      };
    }
    public int X { get; }
    public int Y { get; }
  }
}