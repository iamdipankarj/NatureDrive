using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Solace {
  public abstract class VehicleStandardInput : MonoBehaviour {
    [NonSerialized]
    public float throttleInput;

    [NonSerialized]
    public float steerInput;

    [NonSerialized]
    public float reverseInput;

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

    private void OnReverse(float delta) {
      reverseInput = delta;
    }

    private void OnEnable() {
      InputManager.DidThrottle += OnThrottle;
      InputManager.DidReverse += OnReverse;
      InputManager.DidSteer += OnSteer;
      InputManager.DidUseHandBrake += OnHandBrake;
    }

    private void OnDisable() {
      InputManager.DidThrottle -= OnThrottle;
      InputManager.DidReverse -= OnReverse;
      InputManager.DidSteer -= OnSteer;
      InputManager.DidUseHandBrake -= OnHandBrake;
    }
  }
}
