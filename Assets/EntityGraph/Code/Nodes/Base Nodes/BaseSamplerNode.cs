using GraphProcessor;
using UnityEngine;

namespace OrionReed
{
  public abstract class BaseSamplerNode : BaseNode, ISampler
  {
    [Output(name = "Out")]
    public ISampler output;

    protected override void Process() { output = this; }

    public float SamplePosition(Vector3 position) => Mathf.Clamp01(Sample(position));
    protected abstract float Sample(Vector3 position);
  }
}