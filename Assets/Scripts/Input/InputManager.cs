using UnityEngine.InputSystem;
using UnityEngine;
using System;
using static Solace.InputManager;

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
    public delegate void HandBrakeAction(bool isPressing);
    public static event HandBrakeAction DidHandBrake;

    // Shift Up
    public delegate void ShiftUpAction(bool isPressed);
    public static event ShiftUpAction DidShiftUp;

    // Shift Down
    public delegate void ShiftDownAction(bool isPressed);
    public static event ShiftDownAction DidShiftDown;

    // Left Blinker
    public delegate void LeftBlinkerAction();
    public static event LeftBlinkerAction DidLeftBlinker;

    // Right Blinker
    public delegate void RightBlinkerAction();
    public static event RightBlinkerAction DidRightBlinker;

    // Low Beam Lights
    public delegate void LowBeamLightsAction();
    public static event LowBeamLightsAction DidLowBeamLight;

    // High Beam Lights
    public delegate void HighBeamLightsAction();
    public static event HighBeamLightsAction DidHighBeamLight;

    // Hazard Light
    public delegate void HazardLightAction();
    public static event HazardLightAction DidHazardLight;

    // Horn
    public delegate void HornAction(bool isPressing);
    public static event HornAction DidHorn;

    // Engine Start/Stop
    public delegate void EngineStartStopAction();
    public static event EngineStartStopAction DidEngineStartStop;

    // Engine Start/Stop
    public delegate void GearToggleAction();
    public static event GearToggleAction DidToggleGearSystem;

    // Extra Lights
    public delegate void ExtraLightsAction();
    public static event ExtraLightsAction DidExtraLight;

    // Clutch
    public delegate void ClutchAction(float delta);
    public static event ClutchAction DidClutch;

    // Boost
    public delegate void BoostAction();
    public static event BoostAction DidBoost;

    // Flip Over
    public delegate void FlipOverAction();
    public static event FlipOverAction DidFlipOver;

    // Cruise Control
    public delegate void CruiseControl();
    public static event CruiseControl DidCruiseControl;

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

    // Shift to 7
    public delegate void ShiftInto7Action();
    public static event ShiftInto7Action DidShiftInto7;

    // Shift to 8
    public delegate void ShiftInto8Action();
    public static event ShiftInto8Action DidShiftInto8;

    // Pause
    public delegate void PauseAction();
    public static event PauseAction DidPause;

    // Cinematic Mode
    public delegate void CinematicModeAction(bool isPressing);
    public static event CinematicModeAction DidUseCinematicMode;

    // Camera Switch
    public delegate void CameraSwitchAction();
    public static event CameraSwitchAction DidSwitchCamera;

    // Enter Vehicle Action
    public delegate void EnterVehicleAction();
    public static event EnterVehicleAction DidEnterVehicle;

    // Input Device Events
    // Disconnect
    public delegate void DeviceDisconnectAction();
    public static event DeviceDisconnectAction DidDeviceDisconnect;

    // Reconnect
    public delegate void DeviceReconnectAction();
    public static event DeviceReconnectAction DidDeviceReconnect;

    // Player
    // Jump
    public delegate void JumpAction(bool value);
    public static event JumpAction DidJump;

    // Move
    public delegate void MoveAction(Vector2 delta);
    public static event MoveAction DidMove;

    // Look
    public delegate void PlayerLookAction(Vector2 delta);
    public static event PlayerLookAction DidPlayerLook;

    // Sprint
    public delegate void SprintAction(bool value);
    public static event SprintAction DidSprint;

    // Pickup
    public delegate void InteractAction();
    public static event InteractAction DidInteract;

    public delegate void FocusAction(bool value);
    public static event FocusAction DidFocus;

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

    private void OnVehicleThrottle(InputAction.CallbackContext context) {
      DidThrottle?.Invoke(context.ReadValue<float>());
    }

    private void OnVehicleThrottleCanceled(InputAction.CallbackContext context) {
      DidThrottle?.Invoke(0f);
    }

    private void OnVehicleBrake(InputAction.CallbackContext context) {
      DidBrake?.Invoke(context.ReadValue<float>());
    }

    private void OnVehicleBrakeCanceled(InputAction.CallbackContext context) {
      DidBrake?.Invoke(0f);
    }

    private void OnVehicleHandBrakePress(InputAction.CallbackContext context) {
      DidHandBrake?.Invoke(true);
    }

    private void OnVehicleHandBrakeRelease(InputAction.CallbackContext context) {
      DidHandBrake?.Invoke(false);
    }

    private void OnShiftUp(InputAction.CallbackContext context) {
      DidShiftUp?.Invoke(true);
    }

    private void OnShiftUpCanceled(InputAction.CallbackContext context) {
      DidShiftUp?.Invoke(false);
    }

    private void OnShiftDown(InputAction.CallbackContext context) {
      DidShiftDown?.Invoke(true);
    }

    private void OnShiftDownCanceled(InputAction.CallbackContext context) {
      DidShiftDown?.Invoke(false);
    }

    private void OnVehicleStartStop(InputAction.CallbackContext context) {
      DidEngineStartStop?.Invoke();
    }

    private void OnVehicleBoost(InputAction.CallbackContext context) {
      DidBoost?.Invoke();
    }

    private void OnVehicleFlipOver(InputAction.CallbackContext context) {
      DidFlipOver?.Invoke();
    }

    private void OnCruiseControl(InputAction.CallbackContext context) {
      DidCruiseControl?.Invoke();
    }

    private void OnGearToggle(InputAction.CallbackContext context) {
      DidToggleGearSystem?.Invoke();
    }

    private void OnExtraLights(InputAction.CallbackContext context) {
      DidExtraLight?.Invoke();
    }

    private void OnVehicleClutchPress(InputAction.CallbackContext context) {
      DidClutch?.Invoke(context.ReadValue<float>());
    }

    private void OnVehicleClutchRelease(InputAction.CallbackContext context) {
      DidClutch?.Invoke(0f);
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

    private void OnShiftInto7(InputAction.CallbackContext context) {
      DidShiftInto7?.Invoke();
    }

    private void OnShiftInto8(InputAction.CallbackContext context) {
      DidShiftInto8?.Invoke();
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

    private void OnLowBeamLight(InputAction.CallbackContext context) {
      DidLowBeamLight?.Invoke();
    }

    private void OnHighBeamLight(InputAction.CallbackContext context) {
      DidHighBeamLight?.Invoke();
    }

    private void OnHorn(InputAction.CallbackContext context) {
      DidHorn?.Invoke(true);
    }

    private void OnHornCanceled(InputAction.CallbackContext context) {
      DidHorn?.Invoke(false);
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

    private void OnPlayerMovePerformed(InputAction.CallbackContext context) {
      DidMove?.Invoke(context.ReadValue<Vector2>());
    }

    private void OnPlayerMoveCanceled(InputAction.CallbackContext context) {
      DidMove?.Invoke(context.ReadValue<Vector2>());
    }

    private void OnPlayerLook(InputAction.CallbackContext context) {
      DidPlayerLook?.Invoke(context.ReadValue<Vector2>());
    }

    private void OnPlayerJumpPerformed(InputAction.CallbackContext context) {
      DidJump?.Invoke(true);
    }

    private void OnPlayerJumpCanceled(InputAction.CallbackContext context) {
      DidJump?.Invoke(false);
    }

    private void OnPlayerSprintstarted(InputAction.CallbackContext context) {
      DidSprint?.Invoke(true);
    }

    private void OnPlayerSprintCanceled(InputAction.CallbackContext context) {
      DidSprint?.Invoke(false);
    }

    private void OnPlayerFocusStarted(InputAction.CallbackContext context) {
      DidFocus?.Invoke(true);
    }

    private void OnPlayerFocusCancled(InputAction.CallbackContext context) {
      DidFocus?.Invoke(false);
    }

    private void OnPlayerInteract(InputAction.CallbackContext context) {
      DidInteract?.Invoke();
    }

    private void OnEnterVehicle(InputAction.CallbackContext context) {
      DidEnterVehicle?.Invoke();
    }

    private void OnEnable() {
      controls.Enable();
    }

    void Start() {
      controls.Car.Steer.performed += OnVehicleSteerPerformed;
      controls.Car.Steer.canceled += OnVehicleSteerCanceled;
      controls.Car.Look.performed += OnVehicleLook;

      controls.Car.SwitchCamera.performed += OnCameraSwitchPerformed;

      controls.Car.Throttle.performed += OnVehicleThrottle;
      controls.Car.Throttle.canceled += OnVehicleThrottleCanceled;

      controls.Car.Brakes.performed += OnVehicleBrake;
      controls.Car.Brakes.canceled += OnVehicleBrakeCanceled;

      controls.Car.HandBrake.performed += OnVehicleHandBrakePress;
      controls.Car.HandBrake.canceled += OnVehicleHandBrakeRelease;

      controls.Car.EngineStartStop.performed += OnVehicleStartStop;
      controls.Car.Boost.performed += OnVehicleBoost;
      controls.Car.FlipOver.performed += OnVehicleFlipOver;
      controls.Car.CruiseControl.performed += OnCruiseControl;
      controls.Car.GearToggle.performed += OnGearToggle;
      controls.Car.ExtraLights.performed += OnExtraLights;

      controls.Car.Clutch.performed += OnVehicleClutchPress;
      controls.Car.Clutch.canceled += OnVehicleClutchRelease;

      controls.Car.ShiftUp.performed += OnShiftUp;
      controls.Car.ShiftUp.canceled += OnShiftUpCanceled;

      controls.Car.ShiftDown.performed += OnShiftDown;
      controls.Car.ShiftDown.canceled += OnShiftDownCanceled;

      controls.Car.ShiftIntoR1.performed += OnShiftReverse;
      controls.Car.ShiftInto0.performed += OnShiftInto0;
      controls.Car.ShiftInto1.performed += OnShiftInto1;
      controls.Car.ShiftInto2.performed += OnShiftInto2;
      controls.Car.ShiftInto3.performed += OnShiftInto3;
      controls.Car.ShiftInto4.performed += OnShiftInto4;
      controls.Car.ShiftInto5.performed += OnShiftInto5;
      controls.Car.ShiftInto6.performed += OnShiftInto6;
      controls.Car.ShiftInto7.performed += OnShiftInto7;
      controls.Car.ShiftInto8.performed += OnShiftInto8;

      controls.Car.LeftBlinker.performed += OnLeftBlinker;
      controls.Car.RightBlinker.performed += OnRightBlinker;
      controls.Car.HazardLights.performed += OnHazardLights;

      controls.Car.LowBeamLights.performed += OnLowBeamLight;
      controls.Car.HighBeamLights.performed += OnHighBeamLight;

      controls.Car.Horn.performed += OnHorn;
      controls.Car.Horn.canceled += OnHornCanceled;

      controls.UI.Pause.performed += OnPlayerPause;
      InputSystem.onDeviceChange += InputSystemOnDeviceChange;

      controls.Car.CinematicMode.performed += OnCinematicModeStart;
      controls.Car.CinematicMode.canceled += OnCinematicModeCanceled;

      // Player
      controls.Player.Move.performed += OnPlayerMovePerformed;
      controls.Player.Move.canceled += OnPlayerMoveCanceled;

      controls.Player.Look.performed += OnPlayerLook;

      controls.Player.Jump.performed += OnPlayerJumpPerformed;
      controls.Player.Jump.canceled += OnPlayerJumpCanceled;

      controls.Player.Sprint.started += OnPlayerSprintstarted;
      controls.Player.Sprint.canceled += OnPlayerSprintCanceled;

      controls.Player.Focus.started += OnPlayerFocusStarted;
      controls.Player.Focus.canceled += OnPlayerFocusCancled;

      controls.Player.Interact.performed += OnPlayerInteract;
      controls.Player.EnterVehicle.performed += OnEnterVehicle;
    }

    private void OnDisable() {
      controls.Disable();
      controls.Car.Steer.performed -= OnVehicleSteerPerformed;
      controls.Car.Steer.canceled -= OnVehicleSteerCanceled;
      controls.Car.Look.performed -= OnVehicleLook;

      controls.Car.SwitchCamera.performed -= OnCameraSwitchPerformed;

      controls.Car.Throttle.performed -= OnVehicleThrottle;
      controls.Car.Throttle.canceled -= OnVehicleThrottleCanceled;

      controls.Car.Brakes.performed -= OnVehicleBrake;
      controls.Car.Brakes.canceled -= OnVehicleBrakeCanceled;

      controls.Car.HandBrake.performed -= OnVehicleHandBrakePress;
      controls.Car.HandBrake.canceled -= OnVehicleHandBrakeRelease;

      controls.Car.EngineStartStop.performed -= OnVehicleStartStop;
      controls.Car.Boost.performed -= OnVehicleBoost;
      controls.Car.FlipOver.performed -= OnVehicleFlipOver;
      controls.Car.CruiseControl.performed -= OnCruiseControl;
      controls.Car.GearToggle.performed -= OnGearToggle;
      controls.Car.ExtraLights.performed -= OnExtraLights;

      controls.Car.Clutch.performed -= OnVehicleClutchPress;
      controls.Car.Clutch.canceled -= OnVehicleClutchRelease;

      controls.Car.ShiftUp.performed -= OnShiftUp;
      controls.Car.ShiftUp.canceled -= OnShiftUpCanceled;

      controls.Car.ShiftDown.performed -= OnShiftDown;
      controls.Car.ShiftDown.canceled -= OnShiftDownCanceled;

      controls.Car.ShiftIntoR1.performed -= OnShiftReverse;
      controls.Car.ShiftInto0.performed -= OnShiftInto0;
      controls.Car.ShiftInto1.performed -= OnShiftInto1;
      controls.Car.ShiftInto2.performed -= OnShiftInto2;
      controls.Car.ShiftInto3.performed -= OnShiftInto3;
      controls.Car.ShiftInto4.performed -= OnShiftInto4;
      controls.Car.ShiftInto5.performed -= OnShiftInto5;
      controls.Car.ShiftInto6.performed -= OnShiftInto6;
      controls.Car.ShiftInto7.performed -= OnShiftInto7;
      controls.Car.ShiftInto8.performed -= OnShiftInto8;

      controls.Car.LeftBlinker.performed -= OnLeftBlinker;
      controls.Car.RightBlinker.performed -= OnRightBlinker;
      controls.Car.HazardLights.performed -= OnHazardLights;

      controls.Car.LowBeamLights.performed -= OnLowBeamLight;
      controls.Car.HighBeamLights.performed -= OnHighBeamLight;

      controls.Car.Horn.performed -= OnHorn;
      controls.Car.Horn.canceled -= OnHornCanceled;

      controls.UI.Pause.performed -= OnPlayerPause;
      InputSystem.onDeviceChange -= InputSystemOnDeviceChange;

      controls.Car.CinematicMode.performed -= OnCinematicModeStart;
      controls.Car.CinematicMode.canceled -= OnCinematicModeCanceled;

      // Player
      controls.Player.Move.performed -= OnPlayerMovePerformed;
      controls.Player.Move.canceled -= OnPlayerMoveCanceled;

      controls.Player.Look.performed -= OnPlayerLook;

      controls.Player.Jump.performed -= OnPlayerJumpPerformed;
      controls.Player.Jump.canceled -= OnPlayerJumpCanceled;

      controls.Player.Sprint.started -= OnPlayerSprintstarted;
      controls.Player.Sprint.canceled -= OnPlayerSprintCanceled;

      controls.Player.Focus.started -= OnPlayerFocusStarted;
      controls.Player.Focus.canceled -= OnPlayerFocusCancled;

      controls.Player.Interact.performed -= OnPlayerInteract;
      controls.Player.EnterVehicle.performed -= OnEnterVehicle;
    }
  }
}
