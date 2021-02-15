using UnityEngine;

namespace OrionReed
{
  [ExecuteInEditMode]
  public class EntityGraphPreview : MonoBehaviour
  {
    public EntityGraph graph;

    private void OnDrawGizmos() => graph?.Visualize();
  }
}