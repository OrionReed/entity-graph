using UnityEngine;

namespace OrionReed
{
  public class RaycastDown : IProjector
  {
    private Bounds bounds;

    public RaycastDown(Bounds rayBounds)
    {
      bounds = rayBounds;
    }

    public bool ProjectedPoint(Vector3 initialPoint, out Vector3 result)
    {
      if (Physics.Raycast(initialPoint, Vector3.down, out RaycastHit hit, bounds.size.y))
      {
        result = hit.point;
        return true;
      }
      else
      {
        result = Vector3.zero;
        return false;
      }
    }
  }
}