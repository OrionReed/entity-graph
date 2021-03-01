using GraphProcessor;
using UnityEngine;

namespace OrionReed
{
  public abstract class BaseSamplerNode : BaseNode, IPositionSampler
  {
    [Output(name = "Out")]
    public IPositionSampler output;

    protected override void Process() { output = this; }

    public float SamplePosition(Vector3 position) => Mathf.Clamp01(Sample(position));
    protected abstract float Sample(Vector3 position);
  }
}