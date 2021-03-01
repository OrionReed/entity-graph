using GraphProcessor;
using UnityEngine;

namespace OrionReed
{
  [System.Serializable, NodeMenuItem("Samplers/Gradient")]
  public class SamplerGradientNode : BaseSamplerNode
  {
    [Input(name = "Top"), ShowAsDrawer]
    public float top = 100f;
    [Input(name = "Bottom"), ShowAsDrawer]
    public float bottom = 0f;

    public override string name => "Sample Gradient";

    protected override float Sample(Vector3 position)
    {
      return (float)Mathf.InverseLerp(bottom, top, position.z);
    }
  }
}