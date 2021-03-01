using System;
using System.Collections.Generic;

namespace OrionReed
{
  [Serializable]
  public class CoordinateRNG
  {
    private readonly Dictionary<Coordinate, Random> rng;

    public CoordinateRNG(int seed)
    {
      XXHash.Seed = (uint)seed;
      rng = new Dictionary<Coordinate, Random>();
    }

    public Random this[Coordinate coordinate]
    {
      get
      {
        if (!rng.ContainsKey(coordinate))
        {
          int chunkSeed = unchecked((int)XXHash.GetHash(coordinate.X, coordinate.Y));
          rng[coordinate] = new Random(chunkSeed);
        }

        return rng[coordinate];
      }
    }
  }
}