using System.ArrayExtensions;
using System.Collections.Generic;
using System.Reflection;
using System;
using UnityEngine;

namespace OrionReed
{
  internal static class Util
  {
    public static float RoundDownToDivisor(float num, float divisor) => num - (num % divisor);

    public static float NextFloat(this System.Random random, float min, float max)
    {
      return ((float)random.NextDouble() * (max - min)) + min;
    }

    public static Vector2 NextVector2(this System.Random random, Vector2 min, Vector2 max)
    {
      return new Vector2(random.NextFloat(min.x, max.x), random.NextFloat(min.y, max.y));
    }

    public static Vector2 GetPointInCircle(this System.Random rng, float radius)
    {
      var r = Math.Sqrt((double)rng.Next() / int.MaxValue) * radius;
      var theta = (double)rng.Next() / int.MaxValue * 2 * Math.PI;
      return new Vector2((int)(r * Math.Cos(theta)), (int)(r * Math.Sin(theta)));
    }
  }
}

namespace System
{
  public static class ObjectExtensions
  {
    private static readonly MethodInfo CloneMethod = typeof(Object).GetMethod("MemberwiseClone", BindingFlags.NonPublic | BindingFlags.Instance);

    public static bool IsPrimitive(this Type type)
    {
      if (type == typeof(String))
      {
        return true;
      }

      return type.IsValueType && type.IsPrimitive;
    }

    public static Object Copy(this Object originalObject)
    {
      return InternalCopy(originalObject, new Dictionary<Object, Object>(new ReferenceEqualityComparer()));
    }
    private static Object InternalCopy(Object originalObject, IDictionary<Object, Object> visited)
    {
      if (originalObject == null)
      {
        return null;
      }

      var typeToReflect = originalObject.GetType();
      if (IsPrimitive(typeToReflect))
      {
        return originalObject;
      }

      if (visited.ContainsKey(originalObject))
      {
        return visited[originalObject];
      }

      if (typeof(Delegate).IsAssignableFrom(typeToReflect))
      {
        return null;
      }

      var cloneObject = CloneMethod.Invoke(originalObject, null);
      if (typeToReflect.IsArray)
      {
        var arrayType = typeToReflect.GetElementType();
        if (!IsPrimitive(arrayType))
        {
          Array clonedArray = (Array)cloneObject;
          clonedArray.ForEach((array, indices) => array.SetValue(InternalCopy(clonedArray.GetValue(indices), visited), indices));
        }
      }
      visited.Add(originalObject, cloneObject);
      CopyFields(originalObject, visited, cloneObject, typeToReflect);
      RecursiveCopyBaseTypePrivateFields(originalObject, visited, cloneObject, typeToReflect);
      return cloneObject;
    }

    private static void RecursiveCopyBaseTypePrivateFields(object originalObject, IDictionary<object, object> visited, object cloneObject, Type typeToReflect)
    {
      if (typeToReflect.BaseType != null)
      {
        RecursiveCopyBaseTypePrivateFields(originalObject, visited, cloneObject, typeToReflect.BaseType);
        CopyFields(originalObject, visited, cloneObject, typeToReflect.BaseType, BindingFlags.Instance | BindingFlags.NonPublic, info => info.IsPrivate);
      }
    }

    private static void CopyFields(object originalObject, IDictionary<object, object> visited, object cloneObject, Type typeToReflect, BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.FlattenHierarchy, Func<FieldInfo, bool> filter = null)
    {
      foreach (FieldInfo fieldInfo in typeToReflect.GetFields(bindingFlags))
      {
        if (filter != null && !filter(fieldInfo))
        {
          continue;
        }

        if (IsPrimitive(fieldInfo.FieldType))
        {
          continue;
        }

        var originalFieldValue = fieldInfo.GetValue(originalObject);
        var clonedFieldValue = InternalCopy(originalFieldValue, visited);
        fieldInfo.SetValue(cloneObject, clonedFieldValue);
      }
    }
    public static T Copy<T>(this T original)
    {
      return (T)Copy((Object)original);
    }
  }

  public class ReferenceEqualityComparer : EqualityComparer<Object>
  {
    public override bool Equals(object x, object y)
    {
      return ReferenceEquals(x, y);
    }
    public override int GetHashCode(object obj)
    {
      if (obj == null)
      {
        return 0;
      }

      return obj.GetHashCode();
    }
  }

  namespace ArrayExtensions
  {
    public static class ArrayExtensions
    {
      public static void ForEach(this Array array, Action<Array, int[]> action)
      {
        if (array.LongLength == 0)
        {
          return;
        }

        ArrayTraverse walker = new ArrayTraverse(array);
        do
        {
          action(array, walker.Position);
        }
        while (walker.Step());
      }
    }

    internal class ArrayTraverse
    {
      public int[] Position;
      private readonly int[] maxLengths;

      public ArrayTraverse(Array array)
      {
        maxLengths = new int[array.Rank];
        for (int i = 0; i < array.Rank; ++i)
        {
          maxLengths[i] = array.GetLength(i) - 1;
        }
        Position = new int[array.Rank];
      }

      public bool Step()
      {
        for (int i = 0; i < Position.Length; ++i)
        {
          if (Position[i] < maxLengths[i])
          {
            Position[i]++;
            for (int j = 0; j < i; j++)
            {
              Position[j] = 0;
            }
            return true;
          }
        }
        return false;
      }
    }
  }
}