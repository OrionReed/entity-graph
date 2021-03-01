using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEditor;

namespace OrionReed
{
  [ExecuteAlways]
  [AddComponentMenu("Entity Graph/Entity Projector")]
  public class EntityGraphProjector : MonoBehaviour
  {
    [SerializeField] private EntityGraph graph;

    // Is this region possibly out of date?
    public bool Dirty { get; private set; }
    private EGPointRenderer pointRenderer;
    [SerializeField] private Bounds bounds;

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
    }

    private void OnDisable()
    {
      graph.onExposedParameterListChanged -= SetDirty;
      EntityGraph.ProjectorsInScene.Remove(this);
    }

    private void Update()
    {
      pointRenderer?.Update();
    }

    private void SetDirty() => Dirty = true;
    public void ResetVisualiser(EntityCollection entities)
    {
      Dirty = false;
      pointRenderer = new EGPointRenderer(Bounds, entities, Bounds.center.y + Bounds.extents.y, transform.position);
    }
  }
}