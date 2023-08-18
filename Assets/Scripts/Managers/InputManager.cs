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
    public delegate void AccelerateAction(float delta);
    public static event AccelerateAction DidAccelerate;

    // Reverse
    public delegate void ReverseAction(float delta);
    public static event ReverseAction DidReverse;

    // Hand Brake Use
    public delegate void HandBrakeUseAction(bool isPressing);
    public static event HandBrakeUseAction DidUseHandBrake;

    // Pause
    public delegate void PauseAction();
    public static event PauseAction DidPause;

    // Input Device Events
    // Disconnect
    public delegate void DeviceDisconnectAction();
    public static event DeviceDisconnectAction DidDeviceDisconnect;

    // Reconnect
    public delegate void DeviceReconnectAction();
    public static event DeviceReconnectAction DidDeviceReconnect;

    SolaceInputActions controls;

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
      DidAccelerate?.Invoke(context.ReadValue<float>());
    }

    private void OnVehicleAccelerateCanceled(InputAction.CallbackContext context) {
      DidAccelerate?.Invoke(0f);
    }

    private void OnVehicleReverse(InputAction.CallbackContext context) {
      DidReverse?.Invoke(context.ReadValue<float>());
    }

    private void OnVehicleReverseCanceled(InputAction.CallbackContext context) {
      DidReverse?.Invoke(0f);
    }

    private void OnVehicleHandBrakePress(InputAction.CallbackContext context) {
      DidUseHandBrake?.Invoke(true);
    }

    private void OnVehicleHandBrakeRelease(InputAction.CallbackContext context) {
      DidUseHandBrake?.Invoke(false);
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
      InputSystem.onDeviceChange += InputSystemOnDeviceChange;
    }

    void Start() {
      controls.Car.Steer.performed += OnVehicleSteerPerformed;
      controls.Car.Steer.canceled += OnVehicleSteerCanceled;
      controls.Car.Look.performed += OnVehicleLook;

      controls.Car.Accelerate.performed += OnVehicleAccelerate;
      controls.Car.Accelerate.canceled += OnVehicleAccelerateCanceled;

      controls.Car.Reverse.performed += OnVehicleReverse;
      controls.Car.Reverse.canceled += OnVehicleReverseCanceled;

      controls.Car.HandBrake.performed += OnVehicleHandBrakePress;
      controls.Car.HandBrake.canceled += OnVehicleHandBrakeRelease;

      controls.UI.Pause.performed += OnPlayerPause;
    }

    private void OnDisable() {
      controls.Disable();
      controls.Car.Steer.performed -= OnVehicleSteerPerformed;
      controls.Car.Steer.canceled -= OnVehicleSteerCanceled;
      controls.Car.Look.performed -= OnVehicleLook;

      controls.Car.Accelerate.performed -= OnVehicleAccelerate;
      controls.Car.Accelerate.canceled -= OnVehicleAccelerateCanceled;

      controls.Car.Reverse.performed -= OnVehicleReverse;
      controls.Car.Reverse.canceled -= OnVehicleReverseCanceled;

      controls.Car.HandBrake.performed -= OnVehicleHandBrakePress;
      controls.Car.HandBrake.canceled -= OnVehicleHandBrakeRelease;

      controls.UI.Pause.performed -= OnPlayerPause;
      InputSystem.onDeviceChange -= InputSystemOnDeviceChange;
    }
  }
}
