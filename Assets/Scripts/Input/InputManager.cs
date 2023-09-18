using UnityEngine.InputSystem;
using UnityEngine;
using System;

namespace Solace {
  public class InputManager : MonoBehaviour {
    public static InputManager instance;

    // Steer
    public delegate void SteerAction(float delta);
    public static event SteerAction DidSteer;

    // Look
    public delegate void LookAction(Vector2 delta);
    public static event LookAction DidLook;

    // Accelerate
    public delegate void ThrottleAction(float delta);
    public static event ThrottleAction DidThrottle;

    // Reverse
    public delegate void BrakeAction(float delta);
    public static event BrakeAction DidBrake;

    // Hand Brake Use
    public delegate void HandBrakeUseAction(bool isPressing);
    public static event HandBrakeUseAction DidUseHandBrake;

    // Shift Up
    public delegate void ShiftUpAction();
    public static event ShiftUpAction DidShiftUp;

    // Shift Down
    public delegate void ShiftDownAction();
    public static event ShiftDownAction DidShiftDown;

    // Left Blinker
    public delegate void LeftBlinkerAction();
    public static event LeftBlinkerAction DidLeftBlinker;

    // Right Blinker
    public delegate void RightBlinkerAction();
    public static event RightBlinkerAction DidRightBlinker;

    // Hazard Light
    public delegate void HazardLightAction();
    public static event HazardLightAction DidHazardLight;

    // Horn
    public delegate void HornAction();
    public static event HornAction DidHorn;

    // Engine Start/Stop
    public delegate void EngineStartStopAction();
    public static event EngineStartStopAction DidEngineStartStop;

    // Clutch
    public delegate void ClutchAction(bool isPressing);
    public static event ClutchAction DidClutch;

    // Clutch
    public delegate void BoostAction();
    public static event BoostAction DidBoost;

    // Shift Reverse
    public delegate void ShiftReverseAction();
    public static event ShiftReverseAction DidShiftReverse;

    // Shift Neutral
    public delegate void ShiftNeutralAction();
    public static event ShiftNeutralAction DidShift0;

    // Shift to 1
    public delegate void ShiftInto1Action();
    public static event ShiftInto1Action DidShiftInto1;

    // Shift to 2
    public delegate void ShiftInto2Action();
    public static event ShiftInto2Action DidShiftInto2;

    // Shift to 3
    public delegate void ShiftInto3Action();
    public static event ShiftInto3Action DidShiftInto3;

    // Shift to 4
    public delegate void ShiftInto4Action();
    public static event ShiftInto4Action DidShiftInto4;

    // Shift to 5
    public delegate void ShiftInto5Action();
    public static event ShiftInto5Action DidShiftInto5;

    // Shift to 6
    public delegate void ShiftInto6Action();
    public static event ShiftInto6Action DidShiftInto6;

    // Pause
    public delegate void PauseAction();
    public static event PauseAction DidPause;

    // Cinematic Mode
    public delegate void CinematicModeAction(bool isPressing);
    public static event CinematicModeAction DidUseCinematicMode;

    // Camera Switch
    public delegate void CameraSwitchAction();
    public static event CameraSwitchAction DidSwitchCamera;

    // Input Device Events
    // Disconnect
    public delegate void DeviceDisconnectAction();
    public static event DeviceDisconnectAction DidDeviceDisconnect;

    // Reconnect
    public delegate void DeviceReconnectAction();
    public static event DeviceReconnectAction DidDeviceReconnect;

    private SolaceInputActions controls;

    private void Awake() {
      controls = new();
      if (instance == null) {
        instance = this;
      }
      else {
        Destroy(gameObject);
      }
      DontDestroyOnLoad(gameObject);
    }

    private void OnPlayerPause(InputAction.CallbackContext context) {
      DidPause?.Invoke();
    }

    private void OnVehicleLook(InputAction.CallbackContext context) {
      DidLook?.Invoke(context.ReadValue<Vector2>());
    }

    private void OnVehicleSteerPerformed(InputAction.CallbackContext context) {
      DidSteer?.Invoke(context.ReadValue<float>());
    }

    private void OnVehicleSteerCanceled(InputAction.CallbackContext context) {
      DidSteer?.Invoke(0f);
    }

    private void OnVehicleAccelerate(InputAction.CallbackContext context) {
      DidThrottle?.Invoke(context.ReadValue<float>());
    }

    private void OnVehicleAccelerateCanceled(InputAction.CallbackContext context) {
      DidThrottle?.Invoke(0f);
    }

    private void OnVehicleBrake(InputAction.CallbackContext context) {
      DidBrake?.Invoke(context.ReadValue<float>());
    }

    private void OnVehicleBrakeCanceled(InputAction.CallbackContext context) {
      DidBrake?.Invoke(0f);
    }

    private void OnVehicleHandBrakePress(InputAction.CallbackContext context) {
      DidUseHandBrake?.Invoke(true);
    }

    private void OnVehicleHandBrakeRelease(InputAction.CallbackContext context) {
      DidUseHandBrake?.Invoke(false);
    }

    private void OnShiftUp(InputAction.CallbackContext context) {
      DidShiftUp?.Invoke();
    }

    private void OnShiftDown(InputAction.CallbackContext context) {
      DidShiftDown?.Invoke();
    }

    private void OnVehicleStartStop(InputAction.CallbackContext context) {
      DidEngineStartStop?.Invoke();
    }

    private void OnVehicleBoost(InputAction.CallbackContext context) {
      DidBoost?.Invoke();
    }

    private void OnVehicleClutchPress(InputAction.CallbackContext context) {
      DidClutch?.Invoke(true);
    }

    private void OnVehicleClutchRelease(InputAction.CallbackContext context) {
      DidClutch?.Invoke(false);
    }

    private void OnShiftReverse(InputAction.CallbackContext context) {
      DidShiftReverse?.Invoke();
    }

    private void OnShiftInto0(InputAction.CallbackContext context) {
      DidShift0?.Invoke();
    }

    private void OnShiftInto1(InputAction.CallbackContext context) {
      DidShiftInto1?.Invoke();
    }

    private void OnShiftInto2(InputAction.CallbackContext context) {
      DidShiftInto2?.Invoke();
    }

    private void OnShiftInto3(InputAction.CallbackContext context) {
      DidShiftInto3?.Invoke();
    }

    private void OnShiftInto4(InputAction.CallbackContext context) {
      DidShiftInto4?.Invoke();
    }

    private void OnShiftInto5(InputAction.CallbackContext context) {
      DidShiftInto5?.Invoke();
    }

    private void OnShiftInto6(InputAction.CallbackContext context) {
      DidShiftInto6?.Invoke();
    }

    private void OnLeftBlinker(InputAction.CallbackContext context) {
      DidLeftBlinker?.Invoke();
    }

    private void OnRightBlinker(InputAction.CallbackContext context) {
      DidRightBlinker?.Invoke();
    }

    private void OnHazardLights(InputAction.CallbackContext context) {
      DidHazardLight?.Invoke();
    }

    private void OnHorn(InputAction.CallbackContext context) {
      DidHorn?.Invoke();
    }

    private void OnCinematicModeStart(InputAction.CallbackContext context) {
      DidUseCinematicMode?.Invoke(true);
    }

    private void OnCinematicModeCanceled(InputAction.CallbackContext context) {
      DidUseCinematicMode?.Invoke(false);
    }

    private void OnCameraSwitchPerformed(InputAction.CallbackContext context) {
      DidSwitchCamera?.Invoke();
    }

    private void InputSystemOnDeviceChange(InputDevice device, InputDeviceChange deviceChange) {
      if (deviceChange == InputDeviceChange.Removed || deviceChange == InputDeviceChange.Disconnected) {
        DidDeviceDisconnect?.Invoke();
      }
      else if (deviceChange == InputDeviceChange.Reconnected) {
        DidDeviceReconnect?.Invoke();
      }
    }

    private void OnEnable() {
      controls.Enable();
    }

    void Start() {
      controls.Car.Steer.performed += OnVehicleSteerPerformed;
      controls.Car.Steer.canceled += OnVehicleSteerCanceled;
      controls.Car.Look.performed += OnVehicleLook;

      controls.Car.SwitchCamera.performed += OnCameraSwitchPerformed;

      controls.Car.Throttle.performed += OnVehicleAccelerate;
      controls.Car.Throttle.canceled += OnVehicleAccelerateCanceled;

      controls.Car.Brakes.performed += OnVehicleBrake;
      controls.Car.Brakes.canceled += OnVehicleBrakeCanceled;

      controls.Car.HandBrake.performed += OnVehicleHandBrakePress;
      controls.Car.HandBrake.canceled += OnVehicleHandBrakeRelease;

      controls.Car.EngineSrartStop.performed += OnVehicleStartStop;
      controls.Car.Boost.performed += OnVehicleBoost;

      controls.Car.Clutch.performed += OnVehicleClutchPress;
      controls.Car.Clutch.canceled += OnVehicleClutchRelease;

      controls.Car.ShiftUp.performed += OnShiftUp;
      controls.Car.ShiftDown.performed += OnShiftDown;
      controls.Car.ShiftIntoR1.performed += OnShiftReverse;
      controls.Car.ShiftInto0.performed += OnShiftInto0;
      controls.Car.ShiftInto1.performed += OnShiftInto1;
      controls.Car.ShiftInto2.performed += OnShiftInto2;
      controls.Car.ShiftInto3.performed += OnShiftInto3;
      controls.Car.ShiftInto4.performed += OnShiftInto4;
      controls.Car.ShiftInto5.performed += OnShiftInto5;
      controls.Car.ShiftInto6.performed += OnShiftInto6;

      controls.Car.LeftBlinker.performed += OnLeftBlinker;
      controls.Car.RightBlinker.performed += OnRightBlinker;
      controls.Car.HazardLights.performed += OnHazardLights;

      controls.Car.Horn.performed += OnHorn;

      controls.UI.Pause.performed += OnPlayerPause;
      InputSystem.onDeviceChange += InputSystemOnDeviceChange;

      controls.Car.CinematicMode.performed += OnCinematicModeStart;
      controls.Car.CinematicMode.canceled += OnCinematicModeCanceled;
    }

    private void OnDisable() {
      controls.Disable();
      controls.Car.Steer.performed -= OnVehicleSteerPerformed;
      controls.Car.Steer.canceled -= OnVehicleSteerCanceled;
      controls.Car.Look.performed -= OnVehicleLook;

      controls.Car.SwitchCamera.performed -= OnCameraSwitchPerformed;

      controls.Car.Throttle.performed -= OnVehicleAccelerate;
      controls.Car.Throttle.canceled -= OnVehicleAccelerateCanceled;

      controls.Car.Brakes.performed -= OnVehicleBrake;
      controls.Car.Brakes.canceled -= OnVehicleBrakeCanceled;

      controls.Car.HandBrake.performed -= OnVehicleHandBrakePress;
      controls.Car.HandBrake.canceled -= OnVehicleHandBrakeRelease;

      controls.Car.EngineSrartStop.performed -= OnVehicleStartStop;
      controls.Car.Boost.performed -= OnVehicleBoost;

      controls.Car.Clutch.performed -= OnVehicleClutchPress;
      controls.Car.Clutch.canceled -= OnVehicleClutchRelease;

      controls.Car.ShiftUp.performed -= OnShiftUp;
      controls.Car.ShiftDown.performed -= OnShiftDown;
      controls.Car.ShiftIntoR1.performed -= OnShiftReverse;
      controls.Car.ShiftInto0.performed -= OnShiftInto0;
      controls.Car.ShiftInto1.performed -= OnShiftInto1;
      controls.Car.ShiftInto2.performed -= OnShiftInto2;
      controls.Car.ShiftInto3.performed -= OnShiftInto3;
      controls.Car.ShiftInto4.performed -= OnShiftInto4;
      controls.Car.ShiftInto5.performed -= OnShiftInto5;
      controls.Car.ShiftInto6.performed -= OnShiftInto6;

      controls.Car.LeftBlinker.performed -= OnLeftBlinker;
      controls.Car.RightBlinker.performed -= OnRightBlinker;
      controls.Car.HazardLights.performed -= OnHazardLights;

      controls.Car.Horn.performed -= OnHorn;

      controls.UI.Pause.performed -= OnPlayerPause;
      InputSystem.onDeviceChange -= InputSystemOnDeviceChange;

      controls.Car.CinematicMode.performed -= OnCinematicModeStart;
      controls.Car.CinematicMode.canceled -= OnCinematicModeCanceled;
    }
  }
}
