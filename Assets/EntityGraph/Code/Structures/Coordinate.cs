using System;
using UnityEngine;
using System.Collections.Generic;

namespace OrionReed
{
  [Serializable]
  public struct Coordinate
  {
    public Coordinate(int x, int y)
    {
      X = x;
      Y = y;
    }

    public static Vector3 GetWorldPositionFromCoordinate(Coordinate coordinate)
    {
      float x = coordinate.X * EntityChunkMatrix.chunkSize;
      float z = coordinate.Y * EntityChunkMatrix.chunkSize;
      return new Vector3(x, 0, z);
    }

    public static Coordinate FromWorldSpace(Vector3 position) => FromWorldSpace(position.x, position.z);
    public static Coordinate FromWorldSpace(int x, int y) => FromWorldSpace(x, y);

    public static Coordinate FromWorldSpace(float x, float y)
    {
      CallCounter.Count("Coordinate.FromWorldSpace");
      return new Coordinate(Mathf.FloorToInt(x / EntityChunkMatrix.chunkSize), Mathf.FloorToInt(y / EntityChunkMatrix.chunkSize));
    }

    public static IEnumerable<Coordinate> IntersectsCircle(Vector3 position, float radius)
    {
      Coordinate centerCoord = Coordinate.FromWorldSpace(position);
      int max = (int)(Utils.RoundDownToDivisor(EntityChunkMatrix.chunkSize + radius, EntityChunkMatrix.chunkSize) / EntityChunkMatrix.chunkSize);
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

    public static IEnumerable<Coordinate> IntersectsBounds(Vector3 position, Vector3 bounds)
    {

      // Account for user input being on the chunk boundary
      Vector3 outerBound = position + (bounds * 0.9999f);

      Coordinate startCoord = FromWorldSpace(position);
      Coordinate endCoord = FromWorldSpace(outerBound);

      for (int x = startCoord.X; x <= endCoord.X; x++)
      {
        for (int y = startCoord.Y; y <= endCoord.Y; y++)
        {
          yield return new Coordinate(x, y);
        }
      }
    }

    public int X { get; }
    public int Y { get; }
  }
}