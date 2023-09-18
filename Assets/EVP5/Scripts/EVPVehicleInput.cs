using UnityEngine;

namespace Solace {
  /// <summary>
  /// Input Controller for EVP based vehicles.
  /// </summary>
  public class EVPVehicleInput : VehicleStandardInput {
    private EVP.VehicleController target;

    private void Start() {
      target = GetComponent<EVP.VehicleController>();
    }

    private void FixedUpdate() {
      float steerInput = Mathf.Clamp(base.steerInput, -1.0f, 1.0f);
      bool handbrakeInput = base.handbrakeInput;
      float forwardInput = Mathf.Clamp01(base.throttleInput);
      float reverseInput = Mathf.Clamp01(base.brakeInput);

      float minSpeed = 0.1f;
      float minInput = 0.1f;
      float throttleInput = 0.0f;
      float brakeInput = 0.0f;

      if (target.speed > minSpeed) {
        throttleInput = forwardInput;
        brakeInput = reverseInput;
      } else {
        if (reverseInput > minInput) {
          throttleInput = -reverseInput;
          brakeInput = 0.0f;
        } else if (forwardInput > minInput) {
          if (target.speed < -minSpeed) {
            throttleInput = 0.0f;
            brakeInput = forwardInput;
          } else {
            throttleInput = forwardInput;
            brakeInput = 0;
          }
        }
      }

      // Apply input to vehicle
      target.steerInput = steerInput;
      target.throttleInput = throttleInput;
      target.brakeInput = brakeInput;
      target.handbrakeInput = handbrakeInput ? 1.0f : 0.0f;
    }
  }
}
