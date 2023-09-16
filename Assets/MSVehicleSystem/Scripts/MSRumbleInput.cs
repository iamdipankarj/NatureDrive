using UnityEngine;
using Solace;

namespace MSVehicle {
  [RequireComponent(typeof(MSVehicleController))]
  public class MSRumbleInput : RumbleStandardInput {
    private MSVehicleController vc;
    private void Start() {
      vc = GetComponent<MSVehicleController>();
      RMB_Initialize();
    }

    private void OnDisable() {
      RMB_OnDisable();
    }

    private bool IsColliding {
      get {
        return vc.lastCollisionTime + 0.3f > Time.realtimeSinceStartup;
        //if (vc.lastCollisionTime + 0.3f > Time.realtimeSinceStartup) {
        //  int strength = (int)(vc.lastCollision.impulse.magnitude / (vc.fixedDeltaTime * vc.ms_Rigidbody.mass * 5f));
        //  base.PlayCollision(strength);
        //}
      }
    }

    private void FixedUpdate() {
      float rpm = vc._vehicleSettings.vehicleRPMValue;
      float maxRpm = vc._vehicleSettings.maxVehicleRPM;
      float clampedRpm = Mathf.Clamp01(Mathf.Abs(rpm / maxRpm));
      RMB_FixedUpdate(clampedRpm, false, IsColliding);
    }
  }
}
