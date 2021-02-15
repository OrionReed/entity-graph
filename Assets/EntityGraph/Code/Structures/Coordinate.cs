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

    public static IEnumerable<Coordinate> IntersectsCircle(Vector3 position, float radius)
    {
      Coordinate centerCoord = Coordinate.FromWorldSpace(position);
      int max = (int)(Utils.RoundDownToDivisor(scale + radius, scale) / scale);
      int startX = centerCoord.X - max;
      int endX = centerCoord.X + max;
      int startY = centerCoord.Y - max;
      int endY = centerCoord.Y + max;

      for (int x = startX; x <= endX; x++)
      {
        for (int y = startY; y <= endY; y++)
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

    public int X { get; }
    public int Y { get; }
  }
}