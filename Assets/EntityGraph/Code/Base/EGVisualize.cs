using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using System.Collections.Generic;
using System;

namespace OrionReed
{
  public static class EGVisualize
  {
    public static Mesh CreatePrimitiveMesh(PrimitiveType type)
    {
      GameObject gameObject = GameObject.CreatePrimitive(type);
      Mesh mesh = gameObject.GetComponent<MeshFilter>().sharedMesh;
      SceneView.DestroyImmediate(gameObject);
      return mesh;
    }

    public static void DrawBounds(Bounds bounds, Color color)
    {
      Handles.color = color;
      Handles.DrawWireCube(bounds.center, bounds.size);
    }
  }
}