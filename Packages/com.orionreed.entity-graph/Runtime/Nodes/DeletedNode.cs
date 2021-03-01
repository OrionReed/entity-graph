using System.Collections.Generic;
using System.Linq;
using GraphProcessor;
using UnityEngine;

namespace OrionReed
{
  [System.Serializable]
  public class DeletedNode : BaseNode
  {
    public override string name => "Deleted Node";

    protected override void Process() { }
  }
}