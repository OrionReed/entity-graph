using System;
using UnityEngine;
using System.Collections.Generic;

namespace OrionReed
{
  [Serializable]
  public class Map
  {
    private readonly Dictionary<Coordinate, float[,]> values = new Dictionary<Coordinate, float[,]>();
    private readonly float cellSize;
    private readonly int cellCount;

    public Map(int cellsPerChunk)
    {
      cellCount = cellsPerChunk;
      cellSize = (float)Coordinate.scale / (float)cellsPerChunk;
    }
    public float[,] AddChunk(Coordinate coord)
    {
      return values[coord] = new float[cellCount + 1, cellCount + 1];
    }

    public void SetFromLocalPos(Coordinate coordinate, Vector3 localPos, float value)
    {
      values[coordinate][Mathf.FloorToInt(localPos.x / cellSize), Mathf.FloorToInt(localPos.z / cellSize)] = value;
    }

    public float Sample(Vector3 pos)
    {
      if (values.TryGetValue(Coordinate.FromWorldSpace(pos, out Vector3 insideChunk), out float[,] value))
      {
        return value[(int)(insideChunk.x / cellSize), (int)(insideChunk.z / cellSize)];
      }
      else
      {
        return 0;
      }
    }

    public IEnumerable<KeyValuePair<Coordinate, float[,]>> IterateChunks()
    {
      foreach (KeyValuePair<Coordinate, float[,]> chunk in values)
      {
        yield return chunk;
      }
    }

    public int ChunkCount()
    {
      return values.Count;
    }

    public void ImprintSquare(Vector3 bottomLeft, float size, float value)
    {
      Coordinate startCoord = Coordinate.FromWorldSpace(bottomLeft, out Vector3 localPosStart);
      Coordinate currentCoord = startCoord;

      int startX = LocalIndex(localPosStart.x);
      int startZ = LocalIndex(localPosStart.z);
      int endXAtStart = LocalIndex(localPosStart.x + size);
      int endX = endXAtStart;
      int endZ = LocalIndex(localPosStart.z + size);
      int coordinateRows = endZ / cellCount;
      for (int row = 0; row <= coordinateRows; row++)
      {
        AddChunkIfNull(currentCoord);
        for (int x = startX; x < endX; x++)
        {
          if (x > 1 && (x - 1) % cellCount == 0)
          {
            currentCoord = new Coordinate(currentCoord.X + 1, currentCoord.Y);
            endX -= x;
            x = 0;
            AddChunkIfNull(currentCoord);
          }
          for (int z = startZ; z < endZ; z++)
          {
            if (x > cellCount || z > cellCount) continue;
            values[currentCoord][x, z] = value;
          }
        }
        currentCoord = new Coordinate(startCoord.X, currentCoord.Y + 1);
        startZ = 0;
        endX = endXAtStart;
        if (row == coordinateRows - 1)
          endZ -= coordinateRows * cellCount;
      }
    }

    private int LocalIndex(float value) => Mathf.RoundToInt(value / cellSize);

    private void AddChunkIfNull(Coordinate coord)
    {
      if (!values.TryGetValue(coord, out _))
        AddChunk(coord);
    }
  }
}