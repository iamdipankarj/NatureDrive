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
      base.Initialize();
    }

    private void Update() {
      SW_Update();
      input.steerInput = base.steeringInput;
      input.throttleInput = base.throttleInput;
      input.reverseInput = base.brakeInput;
    }

    private void FixedUpdate() {
    }
  }
}
