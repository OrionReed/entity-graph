using System;

namespace OrionReed
{
  [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
  public class Deprecated : Attribute
  {
    public Deprecated(Type type)
    {
      newType = type;
    }
    public Type newType;
  }
}