using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using UnityEngine.Rendering;

namespace OrionReed
{
  [CustomEditor(typeof(EntityGraphProjector))]
  [CanEditMultipleObjects]
  public class EGProjectorEditor : Editor
  {
    private readonly BoxBoundsHandle boundsHandles = new BoxBoundsHandle();

    protected virtual void OnSceneGUI()
    {
      if (!EntityGraph.debugDrawBounds)
        return;
      EntityGraphProjector projector = (EntityGraphProjector)target;

      boundsHandles.center = projector.Bounds.center;
      boundsHandles.size = projector.Bounds.size;

      EditorGUI.BeginChangeCheck();
      boundsHandles.DrawHandle();
      if (EditorGUI.EndChangeCheck())
      {
        Undo.RecordObject(projector, "Change Bounds");

        projector.Bounds = new Bounds
        {
          center = boundsHandles.center,
          size = boundsHandles.size
        };
        projector.transform.position = boundsHandles.center;
      }
    }
  }
}