using System;
using UnityEngine;
using Rewired;

namespace Solace {
  /// <summary>
  /// Input Controller for Promoteo based vehicles
  /// </summary>
  public class PrometeoVehicleInput : VehicleStandardInput {
    // Rewired
    private Player player;

    [NonSerialized]
    public bool isAcceleratingForward;
    [NonSerialized]
    public float forwardAccelerateDelta;

    [NonSerialized]
    public bool isAcceleratingBackward;
    [NonSerialized]
    public float backwardAccelerateDelta;

    [NonSerialized]
    public bool isTurningLeft;
    [NonSerialized]
    public bool isTurningRight;
    [NonSerialized]
    public float steeringDelta;

    [NonSerialized]
    public bool isPressingHandbrake;
    [NonSerialized]
    public bool isReleasingHandbrake;

    private void Awake() {
      player = ReInput.players.GetPlayer(0);
    }

    private void OnHandBrake(bool isPressing) {
      if (isPressing) {
        isPressingHandbrake = true;
        isReleasingHandbrake = false;
      } else {
        isPressingHandbrake = false;
        isReleasingHandbrake = true;
      }
    }

    private void OnSteer(float delta) {
      if (delta < 0f) {
        isTurningLeft = true;
        isTurningRight = false;
      } else if (delta > 0f) {
        isTurningRight = true;
        isTurningLeft = false;
      } else if (delta.Equals(0f)) {
        isTurningRight = false;
        isTurningLeft = false;
      }
      steeringDelta = Mathf.Abs(delta);
    }

    private void OnAccelerateForward(float delta) {
      isAcceleratingForward = delta > 0f;
      forwardAccelerateDelta = delta;
    }

    private void OnAccelerateBackward(float delta) {
      isAcceleratingBackward = delta > 0f;
      backwardAccelerateDelta = delta;
    }

    private void Update() {
      OnAccelerateForward(player.GetAxis(RewiredUtils.Throttle));
      OnAccelerateBackward(player.GetAxis(RewiredUtils.Brake));
      OnSteer(player.GetAxis(RewiredUtils.Steering));
      OnHandBrake(player.GetButtonDown(RewiredUtils.HandBrake));
    }
  }
}
