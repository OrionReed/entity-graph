using System;
using GraphProcessor;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace OrionReed
{
  public class EGView : BaseGraphView
  {
    public EGView(EditorWindow window) : base(window) { }

    public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
    {
      BuildStackNodeContextualMenu(evt);
      base.BuildContextualMenu(evt);
    }

    protected void BuildStackNodeContextualMenu(ContextualMenuPopulateEvent evt)
    {
      Vector2 position = (evt.currentTarget as VisualElement).ChangeCoordinatesTo(contentViewContainer, evt.localMousePosition);
      evt.menu.AppendAction("New Output Stack", (_) => AddStackNode(new BaseStackNode(position, "Output")), DropdownMenuAction.AlwaysEnabled);
    }
  }
}