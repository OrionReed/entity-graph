using GraphProcessor;
using System;

namespace OrionReed
{
  public abstract class BaseEntityGraphNode : BaseNode
  {
    protected new EntityGraph graph => base.graph as EntityGraph;
    public Random RNG(Coordinate coordinate) => graph.ChunkRandoms[coordinate];
  }
}