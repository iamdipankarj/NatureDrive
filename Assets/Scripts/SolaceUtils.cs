using System;
using UnityEngine;

namespace Solace {
  public class SolaceUtils {
    public static void PrintEnum<T>(T value) {
      Debug.Log($"Will Go To {Enum.GetName(typeof(T), value)}");
    }
  }
}
