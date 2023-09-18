using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Solace;

namespace MSVehicle {
  [RequireComponent(typeof(MSVehicleController))]
  public class MSSteeringWheelInput : SteeringWheelStandardInput {
    private MSVehicleController vc;
    private VehicleStandardInput input;
    private SteeringWheelVehicleData vehicleData = new();

    void Start() {
      vc = GetComponent<MSVehicleController>();
      input = GetComponent<VehicleStandardInput>();
      base.SW_Initialize();
    }

    private void Update() {
      SW_Update();
      input.steerInput = base.steeringInput;
      input.throttleInput = base.throttleInput;
      input.brakeInput = base.brakeInput;
    }

    private void FixedUpdate() {
      vehicleData.vehicleSpeed = vc.KMh;
      SW_FixedUpdate(vehicleData);

      if (vc.lastCollisionTime + 0.3f > Time.realtimeSinceStartup) {
        int strength = (int)(vc.lastCollision.impulse.magnitude / (vc.fixedDeltaTime * vc.ms_Rigidbody.mass * 5f));
        base.PlayCollision(strength);
      }
    }
  }
}
