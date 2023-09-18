using System;
using UnityEngine;

namespace Solace {
  public abstract class VehicleStandardInput : MonoBehaviour {
    [NonSerialized]
    public float throttleInput;

    [NonSerialized]
    public float steerInput;

    [NonSerialized]
    public float brakeInput;

    [NonSerialized]
    public bool handbrakeInput;

    private void OnThrottle(float delta) {
      throttleInput = delta;
    }

    private void OnSteer(float delta) {
      steerInput = delta;
    }

    private void OnHandBrake(bool isPressing) {
      handbrakeInput = isPressing;
    }

    private void OnBrake(float delta) {
      brakeInput = delta;
    }

    private void OnEnable() {
      InputManager.DidThrottle += OnThrottle;
      InputManager.DidBrake += OnBrake;
      InputManager.DidSteer += OnSteer;
      InputManager.DidUseHandBrake += OnHandBrake;
    }

    private void OnDisable() {
      InputManager.DidThrottle -= OnThrottle;
      InputManager.DidBrake -= OnBrake;
      InputManager.DidSteer -= OnSteer;
      InputManager.DidUseHandBrake -= OnHandBrake;
    }
  }
}
