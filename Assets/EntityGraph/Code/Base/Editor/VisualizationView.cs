using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using System.Linq;
using System;
using GraphProcessor;

namespace OrionReed
{
  public class VisualizationView : PinnedElementView
  {
    private EntityGraph graph;
    private Button buttonBounds;
    private Button buttonChunks;
    private Button buttonEntities;
    private Button buttonMaps;
    private Color enabledBorderColor = new Color(0.090f, 0.478f, 0.631f);

    public override bool IsResizable() => false;

    protected override void Initialize(BaseGraphView graphView)
    {
      title = "Visualization";
      graph = graphView.graph as EntityGraph;

      ColorField colorMinField = new ColorField();
      ColorField colorMaxField = new ColorField();
      Slider sliderBrightness = new Slider(0f, 1f);
      buttonBounds = new Button(OnToggleBounds) { name = "ActionButton", text = "Bounds" };
      buttonChunks = new Button(OnToggleChunks) { name = "ActionButton", text = "Chunks" };
      buttonEntities = new Button(OnToggleEntities) { name = "ActionButton", text = "Entities" };
      buttonMaps = new Button(OnToggleMaps) { name = "ActionButton", text = "Maps" };

      sliderBrightness.RegisterValueChangedCallback(x => BrightnessChanged(x.newValue));
      colorMinField.RegisterValueChangedCallback(x => MinColorChanged(x.newValue));
      colorMaxField.RegisterValueChangedCallback(x => MaxColorChanged(x.newValue));

      colorMinField.value = graph.debugMapColorMin;
      colorMaxField.value = graph.debugMapColorMax;
      sliderBrightness.value = graph.debugGizmoBrightness;
      UpdateButtonState(buttonBounds, graph.debugDrawBounds);
      UpdateButtonState(buttonChunks, graph.debugDrawChunks);
      UpdateButtonState(buttonEntities, graph.debugDrawEntities);
      UpdateButtonState(buttonMaps, graph.debugDrawMaps);

      content.Add(buttonEntities);
      content.Add(buttonBounds);
      content.Add(buttonChunks);
      content.Add(buttonMaps);
      content.Add(sliderBrightness);
      content.Add(colorMinField);
      content.Add(colorMaxField);
    }

    private void OnToggleBounds()
    {
      graph.debugDrawBounds = !graph.debugDrawBounds;
      UpdateButtonState(buttonBounds, graph.debugDrawBounds);
    }

    private void OnToggleChunks()
    {
      graph.debugDrawChunks = !graph.debugDrawChunks;
      UpdateButtonState(buttonChunks, graph.debugDrawChunks);
    }

    private void OnToggleEntities()
    {
      graph.debugDrawEntities = !graph.debugDrawEntities;
      UpdateButtonState(buttonEntities, graph.debugDrawEntities);
    }

    private void OnToggleMaps()
    {
      graph.debugDrawMaps = !graph.debugDrawMaps;
      UpdateButtonState(buttonMaps, graph.debugDrawMaps);
    }

    private void MinColorChanged(Color newColor) => graph.debugMapColorMin = newColor;
    private void MaxColorChanged(Color newColor) => graph.debugMapColorMax = newColor;
    private void BrightnessChanged(float newValue) => graph.debugGizmoBrightness = newValue;

    private void UpdateButtonState(Button button, bool highlight)
    {
      if (highlight)
      {
        button.style.borderBottomColor = enabledBorderColor;
        button.style.borderLeftColor = enabledBorderColor;
        button.style.borderRightColor = enabledBorderColor;
        button.style.borderTopColor = enabledBorderColor;
      }
      else
      {
        button.style.borderBottomColor = StyleKeyword.Null;
        button.style.borderLeftColor = StyleKeyword.Null;
        button.style.borderRightColor = StyleKeyword.Null;
        button.style.borderTopColor = StyleKeyword.Null;
      }
    }
  }
}