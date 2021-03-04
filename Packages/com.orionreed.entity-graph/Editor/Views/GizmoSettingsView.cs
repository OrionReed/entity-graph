using UnityEngine;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using GraphProcessor;

namespace OrionReed
{
  public class GizmoSettingsView : PinnedElementView
  {
    private Button buttonBounds;
    private Button buttonChunks;
    private Button buttonEntities;
    private Button buttonMaps;
    private Color enabledColor = new Color(0.35f, 0.35f, 0.35f);
    private Color disabledColor = new Color(0.25f, 0.25f, 0.25f);

    public override bool IsResizable() => false;

    protected override void Initialize(BaseGraphView graphView)
    {
      title = "Gizmo Settings";
      Rect pos = GetPosition();
      SetPosition(new Rect(20, 40, pos.width, pos.height));

      Slider sliderBrightness = new Slider(0f, 1f);
      buttonEntities = new Button(OnToggleEntities) { name = "ActionButton", text = "Entities" };
      buttonBounds = new Button(OnToggleBounds) { name = "ActionButton", text = "Bounds" };
      buttonChunks = new Button(OnToggleChunks) { name = "ActionButton", text = "Chunks" };
      buttonMaps = new Button(OnToggleMaps) { name = "ActionButton", text = "Maps" };

      sliderBrightness.RegisterValueChangedCallback(x => BrightnessChanged(x.newValue));

      sliderBrightness.value = EGWindow.debugGizmoBrightness;
      UpdateButtonState(buttonBounds, EGWindow.debugDrawBounds);
      UpdateButtonState(buttonChunks, EGWindow.debugDrawChunks);
      UpdateButtonState(buttonEntities, EGWindow.debugDrawEntities);
      UpdateButtonState(buttonMaps, EGWindow.debugDrawMaps);

      Label visibilityLabel = new Label("Visibility");
      Label brightnessLabel = new Label("Brightness");
      visibilityLabel.style.paddingTop = new StyleLength(10f);
      brightnessLabel.style.paddingTop = new StyleLength(10f);

      content.Add(visibilityLabel);
      content.Add(buttonEntities);
      content.Add(buttonBounds);
      content.Add(buttonChunks);
      content.Add(buttonMaps);
      content.Add(brightnessLabel);
      content.Add(sliderBrightness);
    }

    private void OnToggleBounds()
    {
      EGWindow.debugDrawBounds = !EGWindow.debugDrawBounds;
      UpdateButtonState(buttonBounds, EGWindow.debugDrawBounds);
    }

    private void OnToggleChunks()
    {
      EGWindow.debugDrawChunks = !EGWindow.debugDrawChunks;
      UpdateButtonState(buttonChunks, EGWindow.debugDrawChunks);
    }

    private void OnToggleEntities()
    {
      EGWindow.debugDrawEntities = !EGWindow.debugDrawEntities;
      UpdateButtonState(buttonEntities, EGWindow.debugDrawEntities);
    }

    private void OnToggleMaps()
    {
      EGWindow.debugDrawMaps = !EGWindow.debugDrawMaps;
      UpdateButtonState(buttonMaps, EGWindow.debugDrawMaps);
    }

    private void BrightnessChanged(float newValue) => EGWindow.debugGizmoBrightness = newValue;

    private void UpdateButtonState(Button button, bool highlight)
    {
      if (highlight)
        button.style.backgroundColor = enabledColor;
      else
        button.style.backgroundColor = disabledColor;
    }
  }
}