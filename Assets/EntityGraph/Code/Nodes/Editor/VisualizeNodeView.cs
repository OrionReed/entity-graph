using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using GraphProcessor;

namespace OrionReed
{
  [NodeCustomEditor(typeof(VisualizeNode))]
  public class VisualizeNodeView : BaseNodeView
  {
    public override void Enable()
    {
      var node = nodeTarget as VisualizeNode;

      Button b = new Button(OnSetVisualization) { text = "teteet" };
      /* var t = new Toggle("Swith type"){ value = node.toggleType };
      t.RegisterValueChangedCallback(e => {
        node.toggleType = e.newValue;
        ForceUpdatePorts();
      }); */

      //var icon = EditorGUIUtility.IconContent("UnityLogo").image;
      //AddMessageView("Custom message !", icon, Color.green);

      controlsContainer.Add(b);
    }

    private void OnSetVisualization()
    {
      Debug.Log("setting viz");
    }
  }
}