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
    [SerializeField] private UnityEngine.VFX.VisualEffect vfx;

    // Is this region possibly out of date?
    public bool Dirty { get; private set; }
    //private EGPointRenderer pointRenderer;
    private EGPointRendererVFX pointRendererVFX;
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
      //pointRenderer?.Update();
    }

    private void SetDirty() => Dirty = true;
    public void ResetVisualiser(EntityCollection entities)
    {
      Dirty = false;
      pointRendererVFX = new EGPointRendererVFX(entities, vfx, bounds);
    }
  }
}