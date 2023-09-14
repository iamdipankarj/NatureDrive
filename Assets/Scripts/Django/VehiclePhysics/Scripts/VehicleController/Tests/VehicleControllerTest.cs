using System.Collections.Generic;
using UnityEngine;
using Django.Common.Vehicles;

#if UNITY_EDITOR
using UnityEditor;
using Django.NUI;
#endif

namespace Django.VehiclePhysics2.Tests
{
    [RequireComponent(typeof(VehicleController))]
    public partial class VehicleControllerTest : MonoBehaviour
    {
        public VehicleController vehicleController;
        private List<VehicleComponent> components = new List<VehicleComponent>();


        private void Awake()
        {
            vehicleController = GetComponent<VehicleController>();
        }


        private void RandomlyEnableDisableComponent()
        {
            int randomIndex = Random.Range(0, components.Count);
            bool enable = Random.Range(0f, 1f) > 0.5f;
            VehicleComponent component = components[randomIndex];

            if (enable)
            {
                Debug.Log($"Enable {component.GetType().Name}");
                component.state.lodIndex = 0;
                component.UpdateLOD();
            }
            else
            {
                Debug.Log($"Disable {component.GetType().Name}");
                component.state.lodIndex = 3;
                component.UpdateLOD();
            }

        }


        public void RunStateTest()
        {
            components = new List<VehicleComponent>();

            // Add main
            components.Add(vehicleController.steering);
            components.Add(vehicleController.powertrain);
            components.Add(vehicleController.brakes);
            components.Add(vehicleController.groundDetection);
            components.Add(vehicleController.moduleManager);

            // Add effects
            components.Add(vehicleController.effectsManager);
            components.AddRange(vehicleController.effectsManager.Components);

            // Add sound
            components.Add(vehicleController.soundManager);
            components.AddRange(vehicleController.soundManager.Components);

            InvokeRepeating("RandomlyEnableDisableComponent", 0.05f, 0.2f);
        }


        public void RunTests()
        {
            RunStateTest();
        }


        public void StopTests()
        {
            CancelInvoke("RandomlyEnableDisableComponent");
        }
    }
}


#if UNITY_EDITOR
namespace Django.VehiclePhysics2.Tests
{
    [CustomEditor(typeof(VehicleControllerTest))]
    [CanEditMultipleObjects]
    public partial class VehicleControllerTestEditor : NVP_NUIEditor
    {
        private FrictionPreset preset;

        public override bool OnInspectorNUI()
        {
            if (!base.OnInspectorNUI())
            {
                return false;
            }

            VehicleControllerTest test = (VehicleControllerTest)target;
            test.vehicleController = test.GetComponent<VehicleController>();

            if (drawer.Button("Run Tests"))
            {
                test.RunTests();
            }

            if (drawer.Button("Stop Tests"))
            {
                test.StopTests();
            }

            drawer.EndEditor(this);
            return true;
        }
    }
}

#endif
