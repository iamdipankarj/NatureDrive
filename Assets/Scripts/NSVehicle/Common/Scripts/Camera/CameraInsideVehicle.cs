using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace NSVehicle
{
    /// <summary>
    ///     Empty component that should be attached to the cameras that are inside the vehicle if interior sound change is to
    ///     be used.
    /// </summary>
    public class CameraInsideVehicle : MonoBehaviour
    {
        /// <summary>
        ///     Is the camera inside vehicle?
        /// </summary>
        [Tooltip("Is the camera inside vehicle?")]
        public bool isInsideVehicle = true;

        private Vehicle _vehicle;

        private void Awake()
        {
            _vehicle = GetComponentInParent<Vehicle>();
            Debug.Assert(_vehicle != null, "CameraInsideVehicle needs to be attached to an object containing a Vehicle script.");
        }

        private void Update()
        {
            _vehicle.CameraInsideVehicle = isInsideVehicle;
        }
    }
}



#if UNITY_EDITOR

namespace NSVehicle
{
    [CustomEditor(typeof(CameraInsideVehicle))]
    [CanEditMultipleObjects]
    public class CameraInsideVehicleEditor : NUIEditor
    {
        public override bool OnInspectorNUI()
        {
            if (!base.OnInspectorNUI())
            {
                return false;
            }


            drawer.Field("isInsideVehicle");

            drawer.EndEditor(this);
            return true;
        }


        public override bool UseDefaultMargins()
        {
            return false;
        }
    }
}
#endif