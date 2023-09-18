#if UNITY_EDITOR
using Django.Common.AssetInfo;
using UnityEditor;

namespace Django.VehiclePhysics
{
    public class InitializationMethodsNVP2 : CommonInitializationMethods
    {
        [InitializeOnLoadMethod]
        static void AddNVP2Defines()
        {
            AddDefines("Django_NVP2");
        }

        [InitializeOnLoadMethod]
        static void ShowNVP2WelcomeWindow()
        {
            ShowWelcomeWindow("Django Vehicle Physics");
        }
    }
}
#endif