using System;
using System.Collections.Generic;

namespace OrionReed
{
  [Serializable]
  public class ChunkRandoms
  {
    private Dictionary<Coordinate, System.Random> chunkRNG;

    public Dictionary<Coordinate, System.Random> ChunkRNG => chunkRNG ??= new Dictionary<Coordinate, System.Random>();

    public ChunkRandoms(int seed)
    {
      XXHash.Seed = (uint)seed;
    }

    public System.Random this[Coordinate coordinate]
    {
      get
      {
        if (!ChunkRNG.ContainsKey(coordinate))
        {
          int chunkSeed = unchecked((int)XXHash.GetHash(coordinate.X, coordinate.Y));
          ChunkRNG[coordinate] = new System.Random(chunkSeed);
        }

        return ChunkRNG[coordinate];
      }
    }
  }
}