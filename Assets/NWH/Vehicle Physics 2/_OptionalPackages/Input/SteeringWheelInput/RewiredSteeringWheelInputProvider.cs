using NWH.Common.Vehicles;
using UnityEngine;
using UnityEngine.Serialization;
using System.Text;
using System.Collections.Generic;
using Rewired;
using Solace;

namespace NWH.VehiclePhysics2.Input {
  /// <summary>
  /// Handles input from steeling wheels such as Logitech, Thrustmaster, etc.
  /// Also calculates force feedback.
  /// </summary>
  public class RewiredSteeringWheelInputProvider : VehicleInputProviderBase {
    /// <summary>
    /// Rewired Player
    /// </summary>
    private Player player;

    /// <summary>
    /// Should all steering filtering and smoothing be ignored?
    /// </summary>
    [Tooltip("Should all steering filtering and smoothing be ignored?")]
    private bool useDirectInput = false;

    /// <summary>
    /// Number by which the input steering value received from the wheel is multiplied.
    /// </summary>
    [Tooltip("Number by which the input steering value received from the wheel is multiplied.")]
    public float steeringSensitivity = 1f;

    /// <summary>
    /// If enabled gears 3 and 4 will be used for D/N/R shifting. Requires Automatic Transmission Reverse Type 
    /// to be set to 'Require Shift Input' and Transmission Type to 'Automatic' or 'Automatic Sequential'.
    /// </summary>
    [Tooltip("If enabled gears 3 and 4 will be used for D/N/R shifting. Requires Automatic Transmission Reverse Type " +
        "to be set to 'Require Shift Input' and Transmission Type to 'Automatic' or 'Automatic Sequential'.")]
    public bool hShifterUse34asDR = false;

    /// <summary>
    /// Target VehicleController.
    /// </summary>
    [Tooltip("Target VehicleController.")]
    public VehicleController vehicleController;

    /// <summary>
    /// Maximum wheel force. This is a hardware settings for the used device.
    /// </summary>
    [Range(0, 100)] public int maximumWheelForce = 100;

    /// <summary>
    /// Multiplier by which the overall force is multiplied. Set to 0 to disable force feedback.
    /// </summary>
    [Tooltip("Multiplier by which the overall force is multiplied. Set to 0 to disable force feedback.")]
    public float overallEffectStrength = 1f;

    /// <summary>
    /// Smoothing of the forces. Set to 0 for most direct feedback.
    /// </summary>
    [Range(0, 0.1f)] public float smoothing = 0f;

    /// <summary>
    /// Range of rotation of the wheel in degrees. Most wheels are capable of 900 degrees
    /// but this will be too slow for most games (except for truck simulators).
    /// 540 is usually used in sim racing.
    /// </summary>
    [FormerlySerializedAs("wheelRange")]
    [Tooltip("Range of rotation of the wheel in degrees. Most wheels are capable of 900 degrees\r\nbut this will be too slow for most games (except for truck simulators).\r\n540 is usually used in sim racing.")]
    [Range(450, 900)]
    public int wheelRotationRange = 540;

    /// <summary>
    /// Fix non-linear input from LogitechSDK.
    /// </summary>
    [Tooltip("Fix non-linear input from LogitechSDK.")]
    public bool linearizeSDKInput = true;

    /// <summary>
    /// Curve that shows how the self aligning torque acts in relation to wheel slip.
    /// Vertical axis is force coefficient, horizontal axis is slip.
    /// </summary>
    [Tooltip("Curve that shows how the self aligning torque acts in relation to wheel slip.\r\nVertical axis is force coefficient, horizontal axis is slip.")]
    public AnimationCurve slipSATCurve = new(
      new Keyframe(0, 0, -0.9f, 25f),
      new Keyframe(0.07f, 1),
      new Keyframe(0.16f, 0.93f, -1f, -1f),
      new Keyframe(1f, 0.2f)
    );

    /// <summary>
    /// Maximum force that can be achieved as a result of self aligning torque.
    /// </summary>
    [Tooltip("Maximum force that can be achieved as a result of self aligning torque.")]
    public float maxSatForce = 80;

    /// <summary>
    /// Slip is multiplied by this value before it is used to evaluate slipSATCurve.
    /// Use to adjust the point at which the wheel will begin to loosen as a result of wheel skid.
    /// </summary>
    [Tooltip("Slip is multiplied by this value before it is used to evaluate slipSATCurve.\r\nUse to adjust the point at which the wheel will begin to loosen as a result of wheel skid.")]
    public float slipMultiplier = 4.6f;

    /// <summary>
    /// Friction when vehicle is stationary or near stationary.
    /// </summary>
    [Tooltip("Friction when vehicle is stationary or near stationary.")]
    public float lowSpeedFriction = 70f;

    /// <summary>
    /// Friction when vehicle is moving.
    /// </summary>
    [Tooltip("Friction when vehicle is moving.")]
    public float friction = 16f;

    /// <summary>
    /// Strength of centering force (the tendency of the steering wheel to center itself).
    /// Also affects the feel of bumps as the steering center moves based on suspension compression (e.g. when
    /// right wheel goes over a bump on the road the steering wheel center moves to the left).
    /// </summary>
    [Tooltip("Strength of centering force (the tendency of the steering wheel to center itself).\r\nAlso affects the feel of bumps as the steering center moves based on suspension compression (e.g. when\r\nright wheel goes over a bump on the road the steering wheel center moves to the left).")]
    public float centeringForceStrength = 60;

    /// <summary>
    /// How much the steering center will move based on difference on compression of suspension on the
    /// left and right side. 
    /// </summary>
    [Range(0, 1)] public float centerPositionDrift = 0.4f;

    /// <summary>
    /// Flips the sign on the steering input.
    /// </summary>
    [Tooltip("Flips the sign on the steering input.")]
    private bool flipSteeringInput = false;

    /// <summary>
    /// Flips the sign on the throttle input.
    /// </summary>
    [Tooltip("Flips the sign on the throttle input.")]
    private bool flipThrottleInput = false;

    /// <summary>
    /// Flips the sign on the brake input.
    /// </summary>
    [Tooltip("Flips the sign on the brake input.")]
    private bool flipBrakeInput = false;

    /// <summary>
    /// Flips the sign on the clutch input.
    /// </summary>
    [Tooltip("Flips the sign on the clutch input.")]
    private bool flipClutchInput = false;

    /// <summary>
    /// Flips the sign on the handbrake input.
    /// </summary>
    [Tooltip("Flips the sign on the handbrake input.")]
    private bool flipHandbrakeInput = false;

    /// <summary>
    /// Index of the input device that should be used. 
    /// Leave at -1 for automatic detection.
    /// </summary>
    public int deviceIndex = -1;

    /// <summary>
    /// If the automatic device search is enabled (deviceIndex = -1),
    /// should only force-feedback devices be considered?
    /// </summary>
    public bool onlyFFBDevices = true;

    /// <summary>
    /// If the automatic device search is enabled (deviceIndex = -1),
    /// should only the devices containing a string from the list be considered?
    /// </summary>
    public List<string> deviceNameContainsWhitelist = new List<string>();

    /// <summary>
    /// Should the automatic device search show debug info in the console about the found devices.
    /// </summary>
    public bool showDeviceDebugInfo = true;

    public string foundDevicesDebugString = "---";


    // Inputs
    private float _steeringInput;
    private float _throttleInput;
    private float _brakeInput;
    private float _clutchInput;
    private float _handbrakeInput = 0;
    private int _shiftIntoInput = -999;
    private bool _shiftUpInput = false;
    private bool _shiftDownInput = false;
    public float throttleDeadzone = 0.02f;
    public float brakeDeadzone = 0.02f;
    public float clutchDeadzone = 0.02f;
    public float handbrakeDeadzone = 0.02f;
    public float steeringDeadzone = 0.00f;

    //Forces
    private float _lowSpeedFrictionForce;
    private float _totalForce = 0;
    private float _satForce;
    private float _frictionForce;
    private float _centeringForce;

    private float _centerPosition;
    private float _prevSteering;

    private ForceFeedbackSettings _ffbSettings;
    private WheelUAPI _leftWheel;
    private WheelUAPI _rightWheel;
    private LogitechGSDK.DIJOYSTATE2ENGINES _wheelInput;
    private float _totalForceVelocity;

    // Vehicle-specific coefficients
    private float _overallCoeff = 1f;
    private float _frictionCoeff = 1f;
    private float _lowSpeedFrictionCoeff = 1f;
    private float _satCoeff = 1f;
    private float _centeringCoeff = 1f;

    // Input detection
    StringBuilder _inputDeviceName;

    public override void Awake() {
      base.Awake();
      player = ReInput.players.GetPlayer(0);
    }

    void Start() {
      LogitechGSDK.LogiSteeringInitialize(false);
      _inputDeviceName = new StringBuilder(256);
    }

    private void Update() {
      // This has to run in Update() as the devices do not get detected by the LogitechSDK until after Start()
      if (deviceIndex < 0) {
        deviceIndex = FindDeviceIndex();
        if (deviceIndex >= 0) {
          GetDeviceName(deviceIndex, ref _inputDeviceName);
          Debug.Log($"Using {_inputDeviceName}. Initializing.");
          InitializeWheel();
        }
      }

      if (deviceIndex < 0 || !WheelIsConnected) {
        return;
      }

      GetWheelInputs();
      SetVehicleInputs();
    }

    void FixedUpdate() {
      if (deviceIndex < 0) {
        return;
      }

      if (vehicleController == null) {
        Debug.Log("VehicleController is not set.");
        return;
      }

      _ffbSettings = vehicleController.GetComponent<ForceFeedbackSettings>();
      if (_ffbSettings == null) {
        _overallCoeff = 1f;
        _frictionCoeff = 1f;
        _lowSpeedFrictionCoeff = 1f;
        _satCoeff = 1f;
        _centeringCoeff = 1f;
      } else {
        _overallCoeff = _ffbSettings.overallCoeff;
        _frictionCoeff = _ffbSettings.frictionCoeff;
        _lowSpeedFrictionCoeff = _ffbSettings.lowSpeedFrictionCoeff;
        _satCoeff = _ffbSettings.satCoeff;
        _centeringCoeff = _ffbSettings.centeringCoeff;
      }

      if (!vehicleController.enabled) {
        ResetForce();
        return;
      }

      float newTotalForce = 0;

      if (WheelIsConnected && LogitechGSDK.LogiUpdate()) {
        vehicleController.steering.useRawInput = useDirectInput;

        _leftWheel = vehicleController.powertrain.wheels[0].wheelUAPI;
        _rightWheel = vehicleController.powertrain.wheels[1].wheelUAPI;

        // Self Aligning Torque
        float leftFactor = _leftWheel.Load / _leftWheel.MaxLoad * _leftWheel.FrictionPreset.BCDE.z;
        float rightFactor = _rightWheel.Load / _rightWheel.MaxLoad * _rightWheel.FrictionPreset.BCDE.z;
        float combinedFactor = leftFactor + rightFactor;
        float totalSlip = _leftWheel.LateralSlip * leftFactor + _rightWheel.LateralSlip * rightFactor;
        float absSlip = totalSlip < 0 ? -totalSlip : totalSlip;
        float slipSign = totalSlip < 0 ? -1f : 1f;
        _satForce = slipSATCurve.Evaluate(absSlip * slipMultiplier) * -slipSign * maxSatForce * combinedFactor * _satCoeff;
        newTotalForce += Mathf.Lerp(0f, _satForce, vehicleController.Speed - 0.4f);

        // Determine target center  position (changes with spring compression)
        _centerPosition = ((_rightWheel.SpringLength / _rightWheel.SpringMaxLength) - (_leftWheel.SpringLength / _leftWheel.SpringMaxLength)) * centerPositionDrift;

        // Calculate centering force
        _centeringForce = (_steeringInput - _centerPosition) * centeringForceStrength * _centeringCoeff;
        newTotalForce += _centeringForce;

        // Low speed friction
        _lowSpeedFrictionForce = Mathf.Lerp(lowSpeedFriction, 0, vehicleController.Speed - 0.2f) * _lowSpeedFrictionCoeff;

        // Friction 
        _frictionForce = friction * _frictionCoeff;

        // Apply friction
        LogitechGSDK.LogiPlayDamperForce(deviceIndex, (int)(_lowSpeedFrictionForce + _frictionForce));

        newTotalForce *= overallEffectStrength * _overallCoeff;
        if (smoothing < 0.001f) {
          _totalForce = newTotalForce;
        } else {
          _totalForce = Mathf.SmoothDamp(_totalForce, newTotalForce, ref _totalForceVelocity, smoothing);
        }

        AddForce(_totalForce);

        _prevSteering = _steeringInput;
      } else {
        vehicleController.steering.useRawInput = false;
      }
    }

    public override void OnDestroy() {
      base.OnDestroy();

      _steeringInput = 0;
      _throttleInput = 0;
      _brakeInput = 0;
      _clutchInput = 0;
      _handbrakeInput = 0;
      _shiftIntoInput = -999;
      _shiftUpInput = false;
      _shiftDownInput = false;
      _lowSpeedFrictionForce = 0;
      _totalForce = 0;
      _satForce = 0;
      _frictionForce = 0;
      _centeringForce = 0;
      _centerPosition = 0;

      LogitechGSDK.LogiSteeringShutdown();
    }


    private void Reset() {
      slipSATCurve = new(
        new Keyframe(0, 0, -0.9f, 25f),
        new Keyframe(0.07f, 1),
        new Keyframe(0.16f, 0.93f, -1f, -1f),
        new Keyframe(1f, 0.2f)
      );
    }

    void InitializeWheel() {
      if (deviceIndex < 0) return;
      LogitechGSDK.LogiControllerPropertiesData currentProperties = new();
      LogitechGSDK.LogiGetCurrentControllerProperties(deviceIndex, ref currentProperties);
      currentProperties.forceEnable = true;
      currentProperties.combinePedals = false;
      currentProperties.gameSettingsEnabled = true;
      currentProperties.defaultSpringEnabled = false;
      currentProperties.defaultSpringGain = 100;
      currentProperties.springGain = 100;
      currentProperties.damperGain = 100;
      currentProperties.overallGain = (int)(maximumWheelForce * 100);
      currentProperties.wheelRange = wheelRotationRange;
      LogitechGSDK.LogiSetPreferredControllerProperties(currentProperties);
    }

    public int FindDeviceIndex() {
      for (int i = 0; i < 16; i++) {
        if (LogitechGSDK.LogiIsConnected(i)) {
          LogitechGSDK.LogiGetFriendlyProductName(i, _inputDeviceName, 256);

          if (showDeviceDebugInfo) {
            foundDevicesDebugString += $"{i}: {_inputDeviceName}; ";
          }

          Debug.Log("----");
          Debug.Log($"Found a connected device: {_inputDeviceName}.");

          if (onlyFFBDevices && !LogitechGSDK.LogiHasForceFeedback(i)) {
            // Only interested in FFB devices, continue if not one.
            Debug.Log("Device has no FFB support. Only FFB devices option selected. Skipping.");
            continue;
          }

          if (deviceNameContainsWhitelist.Count > 0) {
            // There are items in the name filter, check that the device name contains a match.
            Debug.Log($"Checking if device name contains a string from the whitelist.");
            string inputDeviceString = _inputDeviceName.ToString();
            foreach (string match in deviceNameContainsWhitelist) {
              if (inputDeviceString.Contains(match)) {
                return i;
              }
            }
            Debug.Log($"No match found. Skipping.");
          } else {
            // No name filters, return the current index.
            return i;
          }

        }
      }

      if (showDeviceDebugInfo) {
        foundDevicesDebugString = "No devices found.";
      }

      return -1;
    }

    public void GetDeviceName(int index, ref StringBuilder deviceName) {
      if (index < 0) {
        Debug.Log("No device selected / found.");
        return;
      }

      LogitechGSDK.LogiGetFriendlyProductName(index, deviceName, 256);
    }

    private bool WheelIsConnected {
      get { return LogitechGSDK.LogiIsConnected(deviceIndex); }
    }

    private void HandleCollision(Collision collision) {
      int strength = (int)(collision.impulse.magnitude /
               (vehicleController.fixedDeltaTime * vehicleController.vehicleRigidbody.mass * 5f));
      LogitechGSDK.LogiPlayFrontalCollisionForce(deviceIndex, strength);
    }

    void SetVehicleInputs() {
      // Shift Up
      _shiftUpInput = player.GetButtonDown(RewiredUtils.ShiftUp);

      // Shift Down
      _shiftDownInput = player.GetButtonDown(RewiredUtils.ShiftDown);

      _shiftIntoInput = hShifterUse34asDR ? 0 : -999;
      if (hShifterUse34asDR && (player.GetButtonDown(RewiredUtils.ShiftInto3) || player.GetButtonDown(RewiredUtils.ShiftInto4))) {
        if (player.GetButtonDown(RewiredUtils.ShiftInto3)) {
          _shiftIntoInput = 1;
        } else if (player.GetButtonDown(RewiredUtils.ShiftInto4)) {
          _shiftIntoInput = -1;
        } else {
          _shiftIntoInput = 0;
        }
      } else {
        // H-shifter
        if (player.GetButton(RewiredUtils.ShiftIntoR1)) {
          _shiftIntoInput = -1;
        } else if (player.GetButton(RewiredUtils.ShiftInto0)) {
          _shiftIntoInput = 0;
        } else if (player.GetButton(RewiredUtils.ShiftInto1)) {
          _shiftIntoInput = 1;
        } else if (player.GetButton(RewiredUtils.ShiftInto2)) {
          _shiftIntoInput = 2;
        } else if (player.GetButton(RewiredUtils.ShiftInto3)) {
          _shiftIntoInput = 3;
        } else if (player.GetButton(RewiredUtils.ShiftInto4)) {
          _shiftIntoInput = 4;
        } else if (player.GetButton(RewiredUtils.ShiftInto5)) {
          _shiftIntoInput = 5;
        } else if (player.GetButton(RewiredUtils.ShiftInto6)) {
          _shiftIntoInput = 6;
        } else if (player.GetButton(RewiredUtils.ShiftInto7)) {
          _shiftIntoInput = 7;
        } else if (player.GetButton(RewiredUtils.ShiftInto8)) {
          _shiftIntoInput = 8;
        }
      }
    }

    void GetWheelInputs() {
      _wheelInput = LogitechGSDK.LogiGetStateUnity(deviceIndex);

      // Steer angle
      _steeringInput = player.GetAxis(RewiredUtils.Steering) * steeringSensitivity;
      if (flipSteeringInput) _steeringInput = -_steeringInput;
      float steerDelta = _steeringInput - _prevSteering;
      if (_steeringInput < steeringDeadzone && _steeringInput > -steeringDeadzone) {
        _steeringInput = 0f;
      }

      // Throttle
      _throttleInput = player.GetAxis(RewiredUtils.Throttle);
      if (flipThrottleInput) _throttleInput = -_throttleInput;
      if (_throttleInput < throttleDeadzone) _throttleInput = 0f;

      // Brake
      _brakeInput = player.GetAxis(RewiredUtils.Brake);
      if (flipBrakeInput) _brakeInput = -_brakeInput;
      if (_brakeInput < brakeDeadzone) _brakeInput = 0f;

      // Clutch
      _clutchInput = player.GetAxis(RewiredUtils.Clutch);
      if (flipClutchInput) _clutchInput = -_clutchInput;
      if (_clutchInput < clutchDeadzone) _clutchInput = 0f;

      // Handbrake
      _handbrakeInput = player.GetButton(RewiredUtils.HandBrake) ? 1f : 0f;
      if (flipHandbrakeInput) _handbrakeInput = -_handbrakeInput;
      if (_handbrakeInput < handbrakeDeadzone) _handbrakeInput = 0f;
    }

    void AddForce(float force) {
      if (deviceIndex < 0) return;
      LogitechGSDK.LogiPlayConstantForce(deviceIndex, (int)force);
    }

    void ResetForce() {
      if (deviceIndex < 0) return;
      LogitechGSDK.LogiStopConstantForce(deviceIndex);
    }

    void OnApplicationQuit() {
      LogitechGSDK.LogiSteeringShutdown();
    }

    public override bool EngineStartStop() {
      return false;
    }

    public override float Clutch() {
      return _clutchInput;
    }

    public override bool ExtraLights() {
      return false;
    }

    public override bool HighBeamLights() {
      return false;
    }

    public override float Handbrake() {
      return _handbrakeInput;
    }

    public override bool HazardLights() {
      return false;
    }

    public override float Brakes() {
      return _brakeInput;
    }

    public override float Steering() {
      return _steeringInput;
    }

    public override bool Horn() {
      return false;
    }

    public override bool LeftBlinker() {
      return false;
    }

    public override bool LowBeamLights() {
      return false;
    }

    public override bool RightBlinker() {
      return false;
    }

    public override bool ShiftDown() {
      return _shiftDownInput;
    }

    public override int ShiftInto() {
      return _shiftIntoInput;
    }

    public override bool ShiftUp() {
      return _shiftUpInput;
    }

    public override bool TrailerAttachDetach() {
      return false;
    }

    public override float Throttle() {
      return _throttleInput;
    }

    public override bool FlipOver() {
      return false;
    }

    public override bool Boost() {
      return false;
    }

    public override bool CruiseControl() {
      return false;
    }
  }
}
