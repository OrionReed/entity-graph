using GraphProcessor;
using UnityEngine;
using System.Collections.Generic;

namespace OrionReed
{
  [System.Serializable, NodeMenuItem("Operations/Imprint")]
  public class OperationImprint : BaseEntityGraphNode, IPositionSampler
  {
    public override string name => "Imprint";
    public int resolution = 10;
    public float squareSize = 5f;
    public float value = 1f;

    [Input(name = "In")]
    public EntityCollection input;

    [Output(name = "Out")]
    public IPositionSampler output;

    [Output(name = "Map")]
    public Map map;

    public float SamplePosition(Vector3 position)
    {
      return map.Sample(position);
    }

    protected override void Process()
    {
      map = new Map(resolution);
      Vector2 centeredSquareOffset = new Vector2(squareSize / 2, squareSize / 2);

      foreach (IEntity entity in input.AllEntities)
      {
        map.ImprintSquare(entity.Position - centeredSquareOffset, squareSize, value);
      }

      output = this;
    }
  }
}