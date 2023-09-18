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

    [NonSerialized]
    public bool engineStartStop = false;

    [NonSerialized]
    public bool toggleGearInput = false;

    [NonSerialized]
    public float clutch = 0;

    [NonSerialized]
    public bool shiftUp = false;

    [NonSerialized]
    public bool shiftDown = false;

    [NonSerialized]
    public bool leftBlinker = false;

    [NonSerialized]
    public bool rightBlinker = false;

    [NonSerialized]
    public bool hazardLights = false;

    [NonSerialized]
    public int shiftInto = 0;

    [NonSerialized]
    public bool cruiseControl = false;

    [NonSerialized]
    public bool horn = false;

    [NonSerialized]
    public bool lowBeamLight = false;

    [NonSerialized]
    public bool highBeamLight = false;

    [NonSerialized]
    public bool extraLight = false;

    [NonSerialized]
    public bool boost = false;

    [NonSerialized]
    public bool flipOver = false;

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

    private void OnEngineStartStop() {
      engineStartStop = !engineStartStop;
    }

    private void OnExtraLight() {
      extraLight = !extraLight;
    }

    private void OnClutch(float delta) {
      clutch = delta;
    }

    private void OnShiftUp(bool isPressed) {
      shiftUp = isPressed;
    }

    private void OnShiftDown(bool isPressed) {
      shiftDown = isPressed;
    }

    private void OnCruiseControl() {
      cruiseControl = !cruiseControl;
    }

    private void OnHorn(bool isPressing) {
      horn = isPressing;
    }

    private void OnBoost() {
      boost = !boost;
    }

    private void OnFlipOver() {
      flipOver = !flipOver;
    }

    private void OnLowBeamLight() {
      lowBeamLight = !lowBeamLight;
    }

    private void OnHighBeamLight() {
      highBeamLight = !highBeamLight;
    }

    private void OnLeftBlinker() {
      leftBlinker = !leftBlinker;
    }

    private void OnRightBlinker() {
      rightBlinker = !rightBlinker;
    }

    private void OnHazardLight() {
      hazardLights = !hazardLights;
    }

    private void OnGearToggle() {
      toggleGearInput = !toggleGearInput;
    }

    private void OnEnable() {
      InputManager.DidThrottle += OnThrottle;
      InputManager.DidBrake += OnBrake;
      InputManager.DidSteer += OnSteer;
      InputManager.DidHandBrake += OnHandBrake;
      InputManager.DidEngineStartStop += OnEngineStartStop;
      InputManager.DidClutch += OnClutch;
      InputManager.DidShiftUp += OnShiftUp;
      InputManager.DidShiftDown += OnShiftDown;
      InputManager.DidCruiseControl += OnCruiseControl;
      InputManager.DidHorn += OnHorn;
      InputManager.DidBoost += OnBoost;
      InputManager.DidFlipOver += OnFlipOver;
      InputManager.DidLowBeamLight += OnLowBeamLight;
      InputManager.DidHighBeamLight += OnHighBeamLight;
      InputManager.DidLeftBlinker += OnLeftBlinker;
      InputManager.DidRightBlinker += OnRightBlinker;
      InputManager.DidHazardLight += OnHazardLight;
      InputManager.DidToggleGearSystem += OnGearToggle;
      InputManager.DidExtraLight += OnExtraLight;
    }

    private void OnDisable() {
      InputManager.DidThrottle -= OnThrottle;
      InputManager.DidBrake -= OnBrake;
      InputManager.DidSteer -= OnSteer;
      InputManager.DidHandBrake -= OnHandBrake;
      InputManager.DidEngineStartStop -= OnEngineStartStop;
      InputManager.DidClutch -= OnClutch;
      InputManager.DidShiftUp -= OnShiftUp;
      InputManager.DidShiftDown -= OnShiftDown;
      InputManager.DidCruiseControl -= OnCruiseControl;
      InputManager.DidHorn -= OnHorn;
      InputManager.DidBoost -= OnBoost;
      InputManager.DidFlipOver -= OnFlipOver;
      InputManager.DidLowBeamLight -= OnLowBeamLight;
      InputManager.DidHighBeamLight -= OnHighBeamLight;
      InputManager.DidLeftBlinker -= OnLeftBlinker;
      InputManager.DidRightBlinker -= OnRightBlinker;
      InputManager.DidHazardLight -= OnHazardLight;
      InputManager.DidToggleGearSystem -= OnGearToggle;
      InputManager.DidExtraLight -= OnExtraLight;
    }
  }
}
