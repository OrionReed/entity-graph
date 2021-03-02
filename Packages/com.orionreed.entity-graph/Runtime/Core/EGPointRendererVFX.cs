using UnityEditor;
using UnityEngine;
using UnityEngine.VFX;

namespace OrionReed
{
  public class EGPointRendererVFX
  {
    public EGPointRendererVFX(EntityCollection entities, VisualEffect vfx, Bounds bounds)
    {
      vfx.Reinit();
      vfx.SetMesh("PointMesh", MeshFromEntityCollection(entities, bounds));
      vfx.SetVector3("BoundsSize", bounds.size);
      vfx.Play();
    }

    private static Mesh MeshFromEntityCollection(EntityCollection entities, Bounds bounds)
    {
      if (entities.EntityCount == 0)
        Debug.LogWarning("Trying to draw empty entity collection...");

      Mesh result = new Mesh();
      Vector3[] vertices = new Vector3[entities.EntityCount];
      Color32[] colors = new Color32[entities.EntityCount];
      Vector2[] uvs = new Vector2[entities.EntityCount];
      int index = 0;
      foreach (IEntity entity in entities.AllEntities)
      {
        vertices[index] = new Vector3(entity.Position.x, bounds.center.y + bounds.extents.y, entity.Position.y);
        uvs[index] = Vector2.one * entity.Settings.Size;
        colors[index] = entity.Settings.Color;
        index++;
      }

      result.SetVertices(vertices);
      result.SetColors(colors);
      result.SetUVs(0, uvs);

      return result;
    }
  }
}