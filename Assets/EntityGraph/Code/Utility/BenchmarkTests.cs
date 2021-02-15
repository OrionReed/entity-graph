using System;
using UnityEngine;

namespace OrionReed
{
  public static class BenchmarkTests
  {
    [Benchmark]
    public static void FirstTest()
    {
      double result = 0;
      for (int i = 0; i < 10000000; i++)
      {
        result += Math.Sqrt(UnityEngine.Random.value * 10000);
      }
    }
  }
}