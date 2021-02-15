using UnityEngine.UIElements;
using GraphProcessor;
using System.Collections.Generic;

namespace OrionReed
{
  // VERY CLUNKY mostly to get used to building views for this system. complete redo in the future.
  [NodeCustomEditor(typeof(BaseEntityGraphNode))]
  public class BaseEntityGraphNodeView : BaseNodeView
  {
    private BaseEntityGraphNode node;

    private EntityCollection EntityOutput
    {
      get
      {
        foreach (var item in node.outputPorts)
        {
          if (item.fieldInfo.FieldType == typeof(EntityCollection))
            return item.fieldInfo.GetValue(item.fieldOwner) as EntityCollection;
        }
        return null;
      }
    }

    public override void Enable()
    {
      node = nodeTarget as BaseEntityGraphNode;
      DrawDefaultInspector();

      node.onBeforeProcessed += () => DrawDefaultDebug();
      node.onBeforeProcessed += () => VerifyProcessability();
      node.onPortsUpdated += (_) => DrawDefaultDebug();
      node.onProcessed += () => ShowEntityCount();
    }

    private void VerifyProcessability()
    {
      if (HasRequiredInputs() && HasRequiredOutputs())
      {
        UnHighlight();
      }
      else
      {
        Highlight();
      }
    }

    private void DrawDefaultDebug()
    {
      debugContainer.Clear();
      debugContainer.Add(new Label("Compute Order: " + node.computeOrder.ToString()));
    }

    private void ShowEntityCount()
    {
      if (EntityOutput?.EntityCount > 0) debugContainer.Add(new Label("Entities Out: " + EntityOutput?.EntityCount));
    }

    private bool HasRequiredInputs()
    {
      Dictionary<string, bool> requiredFields = new Dictionary<string, bool>();

      for (int i = 0; i < node.inputPorts.Count; i++)
      {
        NodePort port = node.inputPorts[i];
        for (int j = 0; j < port.fieldInfo.GetCustomAttributes(false).Length; j++)
        {
          if (port.fieldInfo.GetCustomAttributes(false)[j].GetType() == typeof(RequiredInput))
            requiredFields[port.fieldName] = true;
        }
      }

      if (requiredFields.Count == 0) return true;

      bool canProcess = true;
      foreach (var view in inputPortViews)
      {
        if (requiredFields.TryGetValue(view.fieldName, out _) && view.connectionCount == 0)
        {
          canProcess = false;
          //view.portData.displayType = typeof(ErrorPort);
          debugContainer.Add(new Label("Missing Input: " + view.portName));
        }
        view.UpdatePortView(view.portData);
      }
      return canProcess;
    }

    private bool HasRequiredOutputs()
    {
      foreach (var view in outputPortViews)
      {
        if (view.fieldType == typeof(EntityCollection) && view.connectionCount == 0)
          return false;
      }
      return true;
    }
  }
}
