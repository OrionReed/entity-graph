using GraphProcessor;
using UnityEngine;

namespace OrionReed
{
  public abstract class BaseEntityNode : BaseNode, IEntityInjector
  {
    [Input(name = "In")]
    public EntityCollection input;
    [Output(name = "Out")]
    public EntityCollection output;
  }

  public interface IEntityInjector
  {

  }
}