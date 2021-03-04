using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using UnityEditor;

namespace OrionReed
{
  [ExecuteAlways]
  [RequireComponent(typeof(VisualEffect))]
  [AddComponentMenu("Entity Graph/Entity Projector")]
  public class EntityGraphProjector : MonoBehaviour
  {
    [SerializeField] private EntityGraph graph;
    [SerializeField] private Bounds bounds;
    private float ProjectorHeight { get => bounds.center.y + bounds.extents.y; }

    // Is this region possibly out of date?
    public bool Dirty { get; private set; }
    //private EGPointRenderer pointRenderer;
    private VisualEffect vfx;
    private IProjector projector;

    public Bounds Bounds
    {
      get
      {
        bounds.center = transform.position;
        return bounds;
      }
      set { bounds = value; }
    }

    public EntityGraph Graph => graph;

    private void OnEnable()
    {
      graph.onExposedParameterListChanged += SetDirty;
      EntityGraph.ProjectorsInScene.Add(this);
      if (vfx == null)
        vfx = GetComponent<VisualEffect>();
      if (projector == null)
        projector = new RaycastDown(bounds);
    }

    private void OnDisable()
    {
      graph.onExposedParameterListChanged -= SetDirty;
      EntityGraph.ProjectorsInScene.Remove(this);
    }

    private void SetDirty() => Dirty = true;

    public void SetVisualization(EntityCollection entities)
    {
      Dirty = false;
      new EGPointRendererVFX(entities, vfx, bounds);
    }

    public void Project(EntityCollection entities)
    {
      float y = ProjectorHeight;
      foreach (IEntity entity in entities.AllEntities)
      {
        if (projector.ProjectedPoint(new Vector3(entity.Position.x, y, entity.Position.y), out Vector3 result))
        {
          entity.Instantiation.Instantiate(result);
        }
      }
    }
  }
}