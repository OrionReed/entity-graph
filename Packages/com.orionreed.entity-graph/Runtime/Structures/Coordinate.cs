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

    public static Vector2 WorldPosition(Coordinate coordinate)
    {
      return new Vector2(coordinate.X * scale, coordinate.Y * scale);
    }
    public static Vector3 FloorVector3(Coordinate coordinate)
    {
      return new Vector3(coordinate.X * scale, 0, coordinate.Y * scale);
    }
    public static Vector2 FloorVector2(Coordinate coordinate)
    {
      return new Vector2(coordinate.X * scale, coordinate.Y * scale);
    }
    public static Vector3 Floor(Vector3 position)
    {
      return new Vector3(FloorFactor(position.x), position.y, FloorFactor(position.z));
    }
    public static Vector3 Ceil(Vector3 position)
    {
      return new Vector3(CeilFactor(position.x), position.y, CeilFactor(position.z));
    }

    private static float CeilFactor(float n)
    {
      return scale * Mathf.Ceil(n / scale);
    }
    private static float FloorFactor(float n)
    {
      return scale * Mathf.Floor(n / scale);
    }

    public static Coordinate FromWorldSpace(Vector3 position) => FromWorldSpace(position.x, position.z);

    public static Coordinate FromWorldSpace(int x, int z) => FromWorldSpace(x, z);
    public static Coordinate FromWorldSpace(float x, float z)
    {
      return new Coordinate(Mathf.FloorToInt(x / scale), Mathf.FloorToInt(z / scale));
    }

    public static Coordinate FromWorldSpace(Vector3 position, out Vector2 positionInsideChunk)
    {
      var result = FromWorldSpace(position);
      positionInsideChunk = new Vector2(position.x, position.z) - WorldPosition(result);
      return result;
    }

    public static IEnumerable<Coordinate> IntersectsCircle(Vector2 position, float radius)
    {
      Coordinate centerCoord = FromWorldSpace(position.x, position.y);
      int max = (int)(Util.RoundDownToDivisor(scale + radius, scale) / scale);

      for (int x = centerCoord.X - max; x <= centerCoord.X + max; x++)
      {
        for (int y = centerCoord.Y - max; y <= centerCoord.Y + max; y++)
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