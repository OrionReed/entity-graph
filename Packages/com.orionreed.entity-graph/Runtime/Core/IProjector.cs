using UnityEngine;

namespace OrionReed
{
  public interface IProjector
  {
    bool ProjectedPoint(Vector3 initialPoint, out Vector3 result);
  }
}