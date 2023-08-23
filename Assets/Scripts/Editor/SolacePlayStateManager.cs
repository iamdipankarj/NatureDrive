using UnityEditor;
using UnityEngine.InputSystem;

namespace Solace {
  [InitializeOnLoad]
  public static class SolacePlayStateManager {
    static SolacePlayStateManager() {
      EditorApplication.playModeStateChanged += LogPlayModeState;
    }

    private static void LogPlayModeState(PlayModeStateChange state) {
      if (state == PlayModeStateChange.EnteredEditMode) {
        InputSystem.ResetHaptics();
      }
    }
  }
}
