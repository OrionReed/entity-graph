using GraphProcessor;
using UnityEngine;

namespace OrionReed
{
  [System.Serializable, NodeMenuItem("Samplers/Perlin Noise")]
  public class SamplerPerlinNode : BaseSamplerNode
  {
    public float scale = 1f;

    public override string name => "Sample Perlin";

    protected override float Sample(Vector3 position)
    {
      return Mathf.PerlinNoise(position.x * scale, position.z * scale);
    }
  }
}