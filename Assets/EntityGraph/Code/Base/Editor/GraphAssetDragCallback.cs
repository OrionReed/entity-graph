using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

namespace OrionReed
{
  [InitializeOnLoad]
  public class GraphAssetDragCallback : Editor
  {
    static GraphAssetDragCallback()
    {
      // Adds a callback for when the hierarchy window processes GUI events
      // for every GameObject in the heirarchy.
      EditorApplication.hierarchyWindowItemOnGUI += HierarchyWindowItemCallback;
    }

    static void HierarchyWindowItemCallback(int pID, Rect pRect)
    {
      // happens when an acceptable item is released over the GUI window
      if (Event.current.type == EventType.DragPerform)
      {
        // get all the drag and drop information ready for processing.
        DragAndDrop.AcceptDrag();
        // used to emulate selection of new objects.
        var selectedObjects = new List<GameObject>();
        // run through each object that was dragged in.

        foreach (var objectRef in DragAndDrop.objectReferences)
        {
          // if the object is the particular asset type...
          if (objectRef is EntityGraph)
          {
            // we create a new GameObject using the asset's name.
            var gameObject = new GameObject(objectRef.name);
            // we attach component X, associated with asset X.
            var componentX = gameObject.AddComponent<EntityGraphPreview>();
            // we place asset X within component X.
            componentX.graph = objectRef as EntityGraph;
            // add to the list of selected objects.
            selectedObjects.Add(gameObject);
          }
        }
        // we didn't drag any assets of type AssetX, so do nothing.
        if (selectedObjects.Count == 0) return;
        // emulate selection of newly created objects.
        Selection.objects = selectedObjects.ToArray();

        // make sure this call is the only one that processes the event.
        Event.current.Use();
      }
    }
  }
}