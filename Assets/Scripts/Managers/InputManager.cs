using UnityEngine.InputSystem;
using UnityEngine;
using System;

namespace Solace {
  public class InputManager : MonoBehaviour {
    public static InputManager instance;

    // Steer
    public delegate void SteerAction(Vector2 delta);
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

    // Hand Brake
    public delegate void HandBrakeAction();
    public static event HandBrakeAction DidPressHandBrake;

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
      DidSteer?.Invoke(context.ReadValue<Vector2>());
    }

    private void OnVehicleSteerCanceled(InputAction.CallbackContext context) {
      DidSteer?.Invoke(context.ReadValue<Vector2>());
    }

    private void OnVehicleAccelerate(InputAction.CallbackContext context) {
      DidAccelerate?.Invoke(context.ReadValue<float>());
    }

    private void OnVehicleReverse(InputAction.CallbackContext context) {
      DidReverse?.Invoke(context.ReadValue<float>());
    }

    private void OnVehicleHandBrake(InputAction.CallbackContext context) {
      DidPressHandBrake?.Invoke();
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
      controls.Car.Reverse.performed += OnVehicleReverse;
      controls.Car.HandBrake.performed += OnVehicleHandBrake;

      controls.UI.Pause.performed += OnPlayerPause;
    }

    private void OnDisable() {
      controls.Disable();



      InputSystem.onDeviceChange -= InputSystemOnDeviceChange;
    }
  }
}
