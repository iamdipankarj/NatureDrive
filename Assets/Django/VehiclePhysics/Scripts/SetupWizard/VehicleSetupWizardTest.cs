using Django.VehiclePhysics.SetupWizard;
using UnityEngine;

namespace Django.VehiclePhysics.Tests
{
    /// <summary>
    ///     Runs VehicleSetupWizard on StartEngine.
    /// </summary>
    public partial class VehicleSetupWizardTest : MonoBehaviour
    {
        private void Start()
        {
            VehicleSetupWizard vsw = GetComponent<VehicleSetupWizard>();
            if (vsw != null)
            {
                VehicleController vc = VehicleSetupWizard.RunSetup(vsw.gameObject, vsw.wheelGameObjects, vsw.wheelControllerType);
                if (vc != null)
                {
                    VehicleSetupWizard.RunConfiguration(vc, vsw.preset);
                }

                Destroy(this);
                Destroy(vsw);
            }
            else
            {
                Debug.LogWarning("VehicleSetupWizard does not exist.");
            }
        }
    }
}