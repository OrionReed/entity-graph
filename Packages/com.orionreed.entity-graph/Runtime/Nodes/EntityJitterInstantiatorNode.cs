using GraphProcessor;
using UnityEngine;
using UnityEditor;

namespace OrionReed
{
  [System.Serializable, NodeMenuItem("Entities/Spawners/Jitter")]
  public class EntityJitterInstantiatorNode : BaseEntityNode
  {
    public override string name => "Jitter Y on creation";

    public GameObject prefab;
    public float amount = 1f;

    protected override void Process()
    {
      foreach (IEntity entity in input.AllEntities)
        entity.Instantiation = new JitterInstantiator(prefab, amount);

      output = input;
    }
  }

  public class JitterInstantiator : IEntityInstantiatable
  {
    private static GameObject prefab;
    private static float jitterAmount;
    public JitterInstantiator(GameObject p, float jitter)
    {
      prefab = p;
      jitterAmount = jitter;
    }

    public bool Instantiate(Vector3 position)
    {
      GameObject o = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
      o.transform.position = position + (Vector3.up * Random.value * jitterAmount);
      return true;
    }
  }
}