using System;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using NWH.NUI;
#endif

namespace NWH.VehiclePhysics2.Modules.AirSteer
{
    /// <summary>
    ///     Module that adds steering to the vehicle while in the air.
    /// </summary>
    [Serializable]
    public partial class AirSteerModule : VehicleComponent
    {
        /// <summary>
        /// Torque applied around the Y axis to steer the vehicle while in the air (nose left/right).
        /// Activated with steering input.
        /// </summary>
        [Tooltip("Torque applied around the Y axis to steer the vehicle while in the air (nose left/right).\r\nActivated with steering input.")]
        public float yawTorque = 10000f;

        /// <summary>
        /// Torque applied around the X axis to steer the vehicle while in the air (nose up, down).
        /// Activated with throttle / brake input.
        /// Torque from the changes in the wheel angular velocity will get applied independently of this setting
        /// by the WheelController.
        /// </summary>
        [Tooltip("Torque applied around the X axis to steer the vehicle while in the air (nose up, down).\r\nActivated with throttle / brake input.\r\nTorque from the changes in the wheel angular velocity will get applied independently of this setting\r\nby the WheelController.")]
        public float pitchTorque = 10000f;


        public override void VC_FixedUpdate()
        {
            base.VC_FixedUpdate();
            if (vehicleController.IsGrounded())
            {
                return;
            }

            Vector3 torque = Vector3.zero;
            torque.x = (vehicleController.input.Throttle - vehicleController.input.Brakes) * pitchTorque;
            torque.y = vehicleController.input.Steering * yawTorque;

            vehicleController.vehicleRigidbody.AddRelativeTorque(torque);
        }
    }
}


#if UNITY_EDITOR
namespace NWH.VehiclePhysics2.Modules.AirSteer
{
    [CustomPropertyDrawer(typeof(AirSteerModule))]
    public partial class AirSteerModuleDrawer : ComponentNUIPropertyDrawer
    {
        public override bool OnNUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (!base.OnNUI(position, property, label))
            {
                return false;
            }

            AirSteerModule airSteerModule = SerializedPropertyHelper.GetTargetObjectOfProperty(property) as AirSteerModule;
            if (airSteerModule == null)
            {
                drawer.EndProperty();
                return false;
            }

            drawer.Field("yawTorque");
            drawer.Field("pitchTorque");


            drawer.EndProperty();
            return true;
        }
    }
}
#endif
