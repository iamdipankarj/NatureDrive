#if UNITY_EDITOR
using Django.Common.AssetInfo;
using UnityEditor;

namespace Django.VehiclePhysics {
  public class InitializationMethodsNVP2 : CommonInitializationMethods {
    [InitializeOnLoadMethod]
    static void AddWC3DDefines() {
      AddDefines("Django_WC3D");
    }

    [InitializeOnLoadMethod]
    static void ShowWC3DWelcomeWindow() {
      ShowWelcomeWindow("Wheel Controller 3D");
    }
  }
}
#endif