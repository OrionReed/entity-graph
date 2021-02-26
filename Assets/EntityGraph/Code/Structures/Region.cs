using System;
using System.Collections.Generic;
using UnityEngine;

namespace OrionReed
{
  [Serializable]
  public class Region
  {
    private static readonly Dictionary<Coordinate, Vector2> coordinateLocationCache = new Dictionary<Coordinate, Vector2>();
    private readonly Dictionary<Coordinate, bool> coordinatesToProcess = new Dictionary<Coordinate, bool>();

    public Region(Bounds bounds)
    {
      foreach (Coordinate coord in Coordinate.InsideBounds(bounds))
      {
        AddCoordIfNotProcessed(coord);
      }
    }

    public Region(EntityGraphVolume volume)
    {
      foreach (Coordinate coord in Coordinate.InsideBounds(volume.GetBounds()))
      {
        AddCoordIfNotProcessed(coord);
      }
    }

    public Region(List<Bounds> bounds)
    {
      Vector3 min = new Vector3();
      Vector3 max = new Vector3();
      foreach (Bounds b in bounds)
      {
        foreach (Coordinate coord in Coordinate.InsideBounds(b))
        {
          AddCoordIfNotProcessed(coord);
        }
        min = Vector3.Min(min, b.min);
        max = Vector3.Max(max, b.max);
      }
    }

    private void AddCoordIfNotProcessed(Coordinate coord)
    {
      if (!coordinatesToProcess.TryGetValue(coord, out _))
        coordinatesToProcess.Add(coord, false);
    }

    public bool IsCoordinateProcessed(Coordinate coordinate)
    {
      if (coordinatesToProcess.TryGetValue(coordinate, out bool processed))
      {
        return processed;
      }
      else
      {
        Debug.LogWarning("Coordinate processed state is unknown because it cannot be found");
        return false;
      }
    }

    public void SetProcessed(Coordinate coordinate, bool isProcessed)
    {
      if (coordinatesToProcess.TryGetValue(coordinate, out bool _))
      {
        coordinatesToProcess[coordinate] = isProcessed;
      }
      else
      {
        Debug.LogWarning("Coordinate processed state cannot be set because it cannot be found");
      }
    }

    public static Vector2 GetWorldSpace(Coordinate coordinate)
    {
      if (coordinateLocationCache.TryGetValue(coordinate, out Vector2 result))
      {
        return result;
      }
      else
      {
        return coordinateLocationCache[coordinate] = Coordinate.WorldPosition(coordinate);
      }
    }

    public IEnumerable<Coordinate> EnumerateCoordinates()
    {
      foreach (Coordinate coord in coordinatesToProcess.Keys)
      {
        yield return coord;
      }
    }

    public IEnumerable<Vector2> EnumerateWorld()
    {
      foreach (Coordinate coord in coordinatesToProcess.Keys)
      {
        yield return GetWorldSpace(coord);
      }
    }
  }
}