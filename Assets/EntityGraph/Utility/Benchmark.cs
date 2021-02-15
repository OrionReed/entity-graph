using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Reflection;

namespace OrionReed
{
  /// <summary>
  /// The attribute to use to mark methods as being
  /// the targets of benchmarking.
  /// </summary>
  [AttributeUsage(AttributeTargets.Method)]
  public class BenchmarkAttribute : Attribute
  {
    public int numTimes = 1;
    public BenchmarkAttribute(int averageOverMultipleRuns = 1)
    {
      numTimes = averageOverMultipleRuns;
    }
  }

  public static class CallCounter
  {
    private static Dictionary<string, int> counts = new Dictionary<string, int>();

    #region public

    public static bool Enabled { get; set; }
    public static void Count(this object obj, [CallerMemberName] string caller = null) { if (Enabled) AddCount($"{obj.GetType().Name}.{caller}"); }
    public static void Count(this Type obj, [CallerMemberName] string caller = null) { if (Enabled) AddCount($"{obj.Name}.{caller}"); }
    public static void Count(this string name, [CallerMemberName] string caller = null) { if (Enabled) AddCount($"{name}.{caller}"); }

    public static void Reset() => counts = new Dictionary<string, int>();


    private static void AddCount(string key)
    {
      if (counts.TryGetValue(key, out _))
        counts[key]++;
      else
        counts[key] = 1;
    }

    public static void Results()
    {
      if (counts.Count == 0) return;
      UnityEngine.Debug.Log(String.Join("\n", counts.OrderByDescending(key => key.Value).Select(x => string.Format(" {0} × {1}", x.Key, x.Value)).Append("------------").Prepend("   <b>Call Counter</b>")));
      Reset();
    }

    #endregion
  }

  public static class BenchmarkStatic
  {
    private const BindingFlags publicStatic = BindingFlags.Public | BindingFlags.Static;

    public static void Run()
    {
      List<Assembly> assemblies = new List<Assembly>()
      {
        Assembly.Load("Assembly-CSharp")
        //Assembly.Load("Assembly-CSharp-Editor")
      };

      foreach (Assembly assem in assemblies)
      {
        foreach (Type type in assem.GetTypes())
        {
          List<(MethodInfo, int)> benchmarkMethods = new List<(MethodInfo, int)>();
          foreach (MethodInfo method in type.GetMethods(publicStatic))
          {
            ParameterInfo[] parameters = method.GetParameters();
            if (parameters != null && parameters.Length != 0)
              continue;

            BenchmarkAttribute attribute = (BenchmarkAttribute)method.GetCustomAttribute(typeof(BenchmarkAttribute), false);
            if (attribute != null)
              benchmarkMethods.Add((method, attribute.numTimes));
          }

          if (benchmarkMethods.Count == 0)
            continue;


          foreach ((MethodInfo, int) run in benchmarkMethods)
          {
            Stopwatch timer = new Stopwatch();
            TimeSpan totalTime = new TimeSpan();
            try
            {
              for (int i = 0; i < run.Item2; i++)
              {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();

                timer.Restart();
                run.Item1.Invoke(null, null);
                timer.Stop();
                totalTime += timer.Elapsed;
              }
              if (run.Item2 > 1)
              {
                UnityEngine.Debug.Log($"{run.Item1.Name} (Avg over {run.Item2} runs): {new TimeSpan(totalTime.Ticks / run.Item2)}");
              }
              else
              {
                UnityEngine.Debug.Log($"{run.Item1.Name}: {totalTime}");
              }
            }
            catch (TargetInvocationException e)
            {
              Exception inner = e.InnerException;
              string message = (inner?.Message) ?? "(No message)";
              UnityEngine.Debug.Log($"  {run.Item1.Name}: Failed ({message})");
            }
          }
        }
      }
    }
  }
}