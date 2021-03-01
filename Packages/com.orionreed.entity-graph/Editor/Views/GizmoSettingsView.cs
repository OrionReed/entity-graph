using UnityEngine;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using GraphProcessor;

namespace OrionReed
{
  public class GizmoSettingsView : PinnedElementView
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
      title = "Gizmo Settings";
      graph = graphView.graph as EntityGraph;
      Rect pos = GetPosition();
      SetPosition(new Rect(20, 40, pos.width, pos.height));

      ColorField mapColorField = new ColorField();
      Slider sliderBrightness = new Slider(0f, 1f);
      buttonBounds = new Button(OnToggleBounds) { name = "ActionButton", text = "Bounds" };
      buttonChunks = new Button(OnToggleChunks) { name = "ActionButton", text = "Chunks" };
      buttonEntities = new Button(OnToggleEntities) { name = "ActionButton", text = "Entities" };
      buttonMaps = new Button(OnToggleMaps) { name = "ActionButton", text = "Maps" };

      sliderBrightness.RegisterValueChangedCallback(x => BrightnessChanged(x.newValue));
      mapColorField.RegisterValueChangedCallback(x => MapColorChanged(x.newValue));

      mapColorField.value = EntityGraph.debugMapColor;
      sliderBrightness.value = EntityGraph.debugGizmoBrightness;
      UpdateButtonState(buttonBounds, EntityGraph.debugDrawBounds);
      UpdateButtonState(buttonChunks, EntityGraph.debugDrawChunks);
      UpdateButtonState(buttonEntities, EntityGraph.debugDrawEntities);
      UpdateButtonState(buttonMaps, EntityGraph.debugDrawMaps);

      content.Add(buttonEntities);
      content.Add(buttonBounds);
      content.Add(buttonChunks);
      content.Add(buttonMaps);
      content.Add(sliderBrightness);
      content.Add(mapColorField);
    }

    private void OnToggleBounds()
    {
      EntityGraph.debugDrawBounds = !EntityGraph.debugDrawBounds;
      UpdateButtonState(buttonBounds, EntityGraph.debugDrawBounds);
    }

    private void OnToggleChunks()
    {
      EntityGraph.debugDrawChunks = !EntityGraph.debugDrawChunks;
      UpdateButtonState(buttonChunks, EntityGraph.debugDrawChunks);
    }

    private void OnToggleEntities()
    {
      EntityGraph.debugDrawEntities = !EntityGraph.debugDrawEntities;
      UpdateButtonState(buttonEntities, EntityGraph.debugDrawEntities);
    }

    private void OnToggleMaps()
    {
      EntityGraph.debugDrawMaps = !EntityGraph.debugDrawMaps;
      UpdateButtonState(buttonMaps, EntityGraph.debugDrawMaps);
    }

    private void MapColorChanged(Color newColor) => EntityGraph.debugMapColor = newColor;
    private void BrightnessChanged(float newValue) => EntityGraph.debugGizmoBrightness = newValue;

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