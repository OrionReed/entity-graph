using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

namespace OrionReed
{
  [Serializable]
  public class Region
  {
    private readonly Dictionary<Coordinate, bool> coordinatesToProcess = new Dictionary<Coordinate, bool>();
    private static readonly Dictionary<Coordinate, Vector3> coordinateLocationCache = new Dictionary<Coordinate, Vector3>();

    public Region(Bounds bounds)
    {
      foreach (Coordinate coord in Coordinate.InsideBounds(bounds))
      {
        if (!coordinatesToProcess.TryGetValue(coord, out _))
          coordinatesToProcess.Add(coord, false);
      }
    }

    public Region(List<Bounds> bounds)
    {
      foreach (Bounds b in bounds)
      {
        foreach (Coordinate coord in Coordinate.InsideBounds(b))
        {
          if (!coordinatesToProcess.TryGetValue(coord, out _))
            coordinatesToProcess.Add(coord, false);
        }
      }
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

    public static Vector3 GetWorldSpace(Coordinate coordinate)
    {
      if (coordinateLocationCache.TryGetValue(coordinate, out Vector3 result))
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

    public IEnumerable<Vector3> EnumerateWorld()
    {
      foreach (Coordinate coord in coordinatesToProcess.Keys)
      {
        yield return GetWorldSpace(coord);
      }
    }
  }
}