using UnityEngine;
using Rewired;

namespace Solace {
  /// <summary>
  /// Input Controller for ESP based vehicles.
  /// </summary>
  public class ESPVehicleInput : MonoBehaviour {
    private ESP.VehicleController target;
    private Player player;

    private void Awake() {
      player = ReInput.players.GetPlayer(0);
    }

    private void Start() {
      target = GetComponent<ESP.VehicleController>();
    }

    private void FixedUpdate() {
      float steerInput = Mathf.Clamp(player.GetAxis(RewiredUtils.Steering), -1.0f, 1.0f);
      bool handbrakeInput = player.GetButtonDown(RewiredUtils.HandBrake);
      float forwardInput = player.GetAxis(RewiredUtils.Throttle);
      float reverseInput = player.GetAxis(RewiredUtils.Brake);

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
