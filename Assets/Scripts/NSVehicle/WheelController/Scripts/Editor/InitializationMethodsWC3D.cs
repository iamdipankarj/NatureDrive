#if UNITY_EDITOR
using UnityEditor;

namespace NSVehicle {
  public class InitializationMethodsNVP2 : CommonInitializationMethods {
    [InitializeOnLoadMethod]
    static void AddWC3DDefines() {
      AddDefines("NSVehicle_WC3D");
    }

    [InitializeOnLoadMethod]
    static void ShowWC3DWelcomeWindow() {
      ShowWelcomeWindow("Wheel Controller 3D");
    }
  }
}
#endif