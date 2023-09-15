using EVP;
using MSVehicle;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Solace {
  public class SteeringWheelStandardInput : MonoBehaviour {
    private VehicleStandardInput input;

    public bool[] buttonDown = new bool[128];
    public bool[] buttonPressed = new bool[128];
    public bool[] buttonWasPressed = new bool[128];

    public enum Axis {
      XPosition,
      YPosition,
      ZPosition,
      XRotatation,
      YRotation,
      ZRotation,
      rglSlider0,
      rglSlider1,
      rglSlider2,
      rglSlider3,
      rglASlider0,
      rglASlider1,
      rglASlider2,
      rglASlider3,
      rglFSlider0,
      rglFSlider1,
      rglFSlider2,
      rglFSlider3,
      rglVSlider0,
      rglVSlider1,
      rglVSlider2,
      rglVSlider3,
      lArx,
      lAry,
      lArz,
      lAx,
      lAy,
      lAz,
      lFRx,
      lFRy,
      lFRz,
      lFx,
      lFy,
      lFz,
      lVRx,
      lVRy,
      lVRz,
      lVx,
      lVy,
      lVz,
      None
    }

    public float steeringSensitivity = 1f;

    /// <summary>
    /// Maximum wheel force. This is a hardware settings for the used device.
    /// </summary>
    [Range(0, 100)]
    public int maximumWheelForce = 100;

    /// <summary>
    /// Multiplier by which the overall force is multiplied. Set to 0 to disable force feedback.
    /// </summary>
    public float overallEffectStrength = 1f;

    /// <summary>
    /// Smoothing of the forces. Set to 0 for most direct feedback.
    /// </summary>
    [Range(0, 0.1f)]
    public float smoothing = 0f;

    /// <summary>
    /// Range of rotation of the wheel in degrees. Most wheels are capable of 900 degrees
    /// but this will be too slow for most games (except for truck simulators).
    /// 540 is usually used in sim racing.
    /// </summary>
    public int wheelRotationRange = 540;

    /// <summary>
    /// Fix non-linear input from LogitechSDK.
    /// </summary>
    public bool linearizeSDKInput = true;

    /// <summary>
    /// Curve that shows how the self aligning torque acts in relation to wheel slip.
    /// Vertical axis is force coefficient, horizontal axis is slip.
    /// </summary>
    public AnimationCurve slipSATCurve = new(
      new Keyframe(0, 0, -0.9f, 25f),
      new Keyframe(0.07f, 1),
      new Keyframe(0.16f, 0.93f, -1f, -1f),
      new Keyframe(1f, 0.2f)
    );

    /// <summary>
    /// Maximum force that can be achieved as a result of self aligning torque.
    /// </summary>
    public float maxSatForce = 80;

    /// <summary>
    /// Slip is multiplied by this value before it is used to evaluate slipSATCurve.
    /// Use to adjust the point at which the wheel will begin to loosen as a result of wheel skid.
    /// </summary>
    public float slipMultiplier = 4.6f;

    /// <summary>
    /// Friction when vehicle is stationary or near stationary.
    /// </summary>
    public float lowSpeedFriction = 70f;

    /// <summary>
    /// Friction when vehicle is moving.
    /// </summary>
    public float friction = 16f;

    /// <summary>
    /// Strength of centering force (the tendency of the steering wheel to center itself).
    /// Also affects the feel of bumps as the steering center moves based on suspension compression (e.g. when
    /// right wheel goes over a bump on the road the steering wheel center moves to the left).
    /// </summary>
    public float centeringForceStrength = 60;

    /// <summary>
    /// How much the steering center will move based on difference on compression of suspension on the
    /// left and right side. 
    /// </summary>
    [Range(0, 1)]
    public float centerPositionDrift = 0.4f;

    /// <summary>
    /// Axis resolution of the wheel's ADC.
    /// </summary>
    public int axisResolution = 65536;

    /// <summary>
    /// Flips the sign on the steering input.
    /// </summary>
    public bool flipSteeringInput = false;

    /// <summary>
    /// Flips the sign on the throttle input.
    /// </summary>
    public bool flipThrottleInput = true;

    /// <summary>
    /// Flips the sign on the brake input.
    /// </summary>
    public bool flipBrakeInput = true;

    /// <summary>
    /// Flips the sign on the clutch input.
    /// </summary>
    public bool flipClutchInput = true;

    /// <summary>
    /// Flips the sign on the handbrake input.
    /// </summary>
    public bool flipHandbrakeInput = true;

    /// <summary>
    /// Determines which wheel axis will be used for steering.
    /// </summary>
    public Axis steeringAxis = Axis.XPosition;

    /// <summary>
    /// Determines which wheel axis will be used for throttle.
    /// </summary>
    public Axis throttleAxis = Axis.YPosition;

    public bool throttleZeroToOne = true;

    /// <summary>
    /// Determines which wheel axis will be used for braking.
    /// </summary>
    [UnityEngine.Tooltip("Determines which wheel axis will be used for braking.")]
    public Axis brakeAxis = Axis.ZRotation;

    public bool brakeZeroToOne = true;

    /// <summary>
    /// Determines which wheel axis will be used for clutch.
    /// </summary>
    public Axis clutchAxis = Axis.ZPosition;

    public bool clutchZeroToOne = true;

    /// <summary>
    /// Determines which wheel axis will be used for steering.
    /// If there is no analog axis for handbrake, handbrakeButton mapping can be used instead.
    /// </summary>
    public Axis handbrakeAxis = Axis.None;

    public bool handbrakeZeroToOne = true;

    /// <summary>
    /// Primary shift up button.
    /// </summary>
    public int shiftUpButton = 12;

    /// <summary>
    /// Primary shift down button.
    /// </summary>
    public int shiftDownButton = 13;

    /// <summary>
    /// Alternative shift up button.
    /// To be used when there is both a sequential stick shifter and paddles.
    /// </summary>
    public int altShiftUpButton = 4;

    /// <summary>
    /// Alternative shift down button.
    /// To be used when there is both a sequential stick shifter and paddles.
    /// </summary>
    public int altShiftDownButton = 5;

    /// <summary>
    /// Button used to shift into reverse gear.
    /// </summary>
    public int shiftIntoReverseButton = -1;

    /// <summary>
    /// Button used to shift into neutral gear.
    /// </summary>
    public int shiftIntoNeutralButton = -1;

    /// <summary>
    /// Button used to shift into 1st gear.
    /// Set to -1 to disable.
    /// </summary>

    public int shiftInto1stButton = -1;

    /// <summary>
    /// Button used to shift into 2nd gear.
    /// Set to -1 to disable.
    /// </summary>
    public int shiftInto2ndButton = -1;

    /// <summary>
    /// Button used to shift into 3rd gear.
    /// Set to -1 to disable.
    /// </summary>
    public int shiftInto3rdButton = -1;

    /// <summary>
    /// Button used to shift into 4th gear.
    /// Set to -1 to disable.
    /// </summary>
    public int shiftInto4thButton = -1;

    /// <summary>
    /// Button used to shift into 5th gear.
    /// Set to -1 to disable.
    /// </summary>
    public int shiftInto5thButton = -1;

    /// <summary>
    /// Button used to shift into 6th gear.
    /// Set to -1 to disable.
    /// </summary>
    public int shiftInto6thButton = -1;

    /// <summary>
    /// Button used to trigger handbrake.
    /// For analog input use handbrakeAxis mapping instead.
    /// </summary>
    public int handbrakeButton = -1;

    // Inputs
    [SerializeField][Range(-1, 1)] private float _steeringInput;
    [SerializeField][Range(0, 1)] private float _throttleInput;
    [SerializeField][Range(0, 1)] private float _brakeInput;
    [SerializeField][Range(0, 1)] private float _clutchInput;
    [SerializeField][Range(0, 1)] private float _handbrakeInput = 0;
    [SerializeField] private int _shiftIntoInput = -999;
    [SerializeField] private bool _shiftUpInput = false;
    [SerializeField] private bool _shiftDownInput = false;

    //Forces
    [SerializeField][Range(0, 100)] private float _lowSpeedFrictionForce;
    [SerializeField][Range(0, 100)] private float _totalForce = 0;
    [SerializeField][Range(0, 100)] private float _satForce;
    [SerializeField][Range(0, 100)] private float _frictionForce;
    [SerializeField][Range(0, 100)] private float _centeringForce;

    // Steering forces from vehicle controller
    private float _centerPosition;
    private bool _steeringWheelConnected;
    private float _prevSteering;
    private float _steerVelocity;
    private ForceFeedbackSettings _ffbSettings;
    private LogitechGSDK.LogiControllerPropertiesData _properties;
    private LogitechGSDK.DIJOYSTATE2ENGINES _wheelInput;
    private float _totalForceVelocity;

    // Vehicle-specific coefficients
    private float _overallCoeff = 1f;
    private float _frictionCoeff = 1f;
    private float _lowSpeedFrictionCoeff = 1f;
    private float _satCoeff = 1f;
    private float _centeringCoeff = 1f;

    private void ResetInputsAndForces() {
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
    }

    /// <summary>
    /// Is the steering wheel currently connected? Read only.
    /// </summary>
    public bool SteeringWheelConnected {
      get { return _steeringWheelConnected; }
    }

    private float GetAxisValue(Axis axis, LogitechGSDK.DIJOYSTATE2ENGINES wheelState, bool zeroToOne) {
      float rawValue = 0;
      switch (axis) {
        case Axis.XPosition:
          rawValue = wheelState.lX;
          break;
        case Axis.YPosition:
          rawValue = wheelState.lY;
          break;
        case Axis.ZPosition:
          rawValue = wheelState.lZ;
          break;
        case Axis.XRotatation:
          rawValue = wheelState.lRx;
          break;
        case Axis.YRotation:
          rawValue = wheelState.lRy;
          break;
        case Axis.ZRotation:
          rawValue = wheelState.lRz;
          break;
        case Axis.rglSlider0:
          rawValue = wheelState.rglSlider[0];
          break;
        case Axis.rglSlider1:
          rawValue = wheelState.rglSlider[1];
          break;
        case Axis.rglSlider2:
          rawValue = wheelState.rglSlider[2];
          break;
        case Axis.rglSlider3:
          rawValue = wheelState.rglSlider[3];
          break;
        case Axis.rglASlider0:
          rawValue = wheelState.rglASlider[0];
          break;
        case Axis.rglASlider1:
          rawValue = wheelState.rglASlider[1];
          break;
        case Axis.rglASlider2:
          rawValue = wheelState.rglASlider[2];
          break;
        case Axis.rglASlider3:
          rawValue = wheelState.rglASlider[3];
          break;
        case Axis.rglFSlider0:
          rawValue = wheelState.rglFSlider[0];
          break;
        case Axis.rglFSlider1:
          rawValue = wheelState.rglFSlider[1];
          break;
        case Axis.rglFSlider2:
          rawValue = wheelState.rglFSlider[2];
          break;
        case Axis.rglFSlider3:
          rawValue = wheelState.rglFSlider[3];
          break;
        case Axis.rglVSlider0:
          rawValue = wheelState.rglVSlider[0];
          break;
        case Axis.rglVSlider1:
          rawValue = wheelState.rglVSlider[1];
          break;
        case Axis.rglVSlider2:
          rawValue = wheelState.rglVSlider[2];
          break;
        case Axis.rglVSlider3:
          rawValue = wheelState.rglVSlider[3];
          break;
        case Axis.lArx:
          rawValue = wheelState.lARx;
          break;
        case Axis.lAry:
          rawValue = wheelState.lARy;
          break;
        case Axis.lArz:
          rawValue = wheelState.lARz;
          break;
        case Axis.lAx:
          rawValue = wheelState.lAX;
          break;
        case Axis.lAy:
          rawValue = wheelState.lAY;
          break;
        case Axis.lAz:
          rawValue = wheelState.lAZ;
          break;
        case Axis.lFRx:
          rawValue = wheelState.lFRx;
          break;
        case Axis.lFRy:
          rawValue = wheelState.lFRy;
          break;
        case Axis.lFRz:
          rawValue = wheelState.lFRz;
          break;
        case Axis.lFx:
          rawValue = wheelState.lFX;
          break;
        case Axis.lFy:
          rawValue = wheelState.lFY;
          break;
        case Axis.lFz:
          rawValue = wheelState.lFZ;
          break;
        case Axis.lVRx:
          rawValue = wheelState.lVRx;
          break;
        case Axis.lVRy:
          rawValue = wheelState.lVRy;
          break;
        case Axis.lVRz:
          rawValue = wheelState.lVRz;
          break;
        case Axis.lVx:
          rawValue = wheelState.lVX;
          break;
        case Axis.lVy:
          rawValue = wheelState.lVY;
          break;
        case Axis.lVz:
          rawValue = wheelState.lVZ;
          break;
        case Axis.None:
          break;
        default:
          rawValue = 0;
          break;
      }

      float halfResolution = axisResolution / 2f;
      if (zeroToOne) {
        return (rawValue - halfResolution) / axisResolution;
      } else {
        return rawValue / halfResolution;
      }
    }

    void UpdateWheelSettings() {
      LogitechGSDK.LogiControllerPropertiesData currentProperties = new();
      LogitechGSDK.LogiGetCurrentControllerProperties(0, ref currentProperties);
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

    private void InitializeLogitech() {
      LogitechGSDK.LogiSteeringInitialize(false);
      UpdateWheelSettings();
    }

    private void InitializeForceFeedbackSettings() {
      _ffbSettings = GetComponent<ForceFeedbackSettings>();
      if (_ffbSettings == null) {
        _overallCoeff = 1f;
        _frictionCoeff = 1f;
        _lowSpeedFrictionCoeff = 1f;
        _satCoeff = 1f;
        _centeringCoeff = 1f;
      }
    }

    private void UpdateForceFeedback() {
      _overallCoeff = _ffbSettings.overallCoeff;
      _frictionCoeff = _ffbSettings.frictionCoeff;
      _lowSpeedFrictionCoeff = _ffbSettings.lowSpeedFrictionCoeff;
      _satCoeff = _ffbSettings.satCoeff;
      _centeringCoeff = _ffbSettings.centeringCoeff;
    }

    private void FixedUpdate() {
      UpdateForceFeedback();
      float newTotalForce = 0;

      //if (WheelIsConnected && LogitechGSDK.LogiUpdate()) {
      //  _steeringWheelConnected = true;

      //  vehicleController.steering.useRawInput = useDirectInput;

      //  _leftWheel = vehicleController.powertrain.wheels[0].wheelUAPI;
      //  _rightWheel = vehicleController.powertrain.wheels[1].wheelUAPI;

      //  // Self Aligning Torque
      //  float leftFactor = _leftWheel.Load / 12000f * _leftWheel.FrictionPreset.BCDE.z;
      //  float rightFactor = _rightWheel.Load / 12000f * _rightWheel.FrictionPreset.BCDE.z;
      //  float combinedFactor = leftFactor + rightFactor;
      //  float totalSlip = _leftWheel.LateralSlip * leftFactor + _rightWheel.LongitudinalSlip * rightFactor;
      //  float absSlip = totalSlip < 0 ? -totalSlip : totalSlip;
      //  float slipSign = totalSlip < 0 ? -1f : 1f;
      //  _satForce = slipSATCurve.Evaluate(absSlip * slipMultiplier) * -slipSign * maxSatForce * combinedFactor * _satCoeff;
      //  newTotalForce += Mathf.Lerp(0f, _satForce, vehicleController.Speed - 0.4f);

      //  // Determine target center  position (changes with spring compression)
      //  _centerPosition = ((_rightWheel.SpringLength / _rightWheel.SpringMaxLength) - (_leftWheel.SpringLength / _leftWheel.SpringMaxLength)) * centerPositionDrift;

      //  // Calculate centering force
      //  _centeringForce = (_steeringInput - _centerPosition) * centeringForceStrength * _centeringCoeff;
      //  newTotalForce += _centeringForce;

      //  // Low speed friction
      //  _lowSpeedFrictionForce = Mathf.Lerp(lowSpeedFriction, 0, vehicleController.Speed - 0.2f) * _lowSpeedFrictionCoeff;

      //  // Friction 
      //  _frictionForce = friction * _frictionCoeff;

      //  // Apply friction
      //  LogitechGSDK.LogiPlayDamperForce(0, (int)(_lowSpeedFrictionForce + _frictionForce));

      //  newTotalForce *= overallEffectStrength * _overallCoeff;
      //  if (smoothing < 0.001f) {
      //    _totalForce = newTotalForce;
      //  } else {
      //    _totalForce = Mathf.SmoothDamp(_totalForce, newTotalForce, ref _totalForceVelocity, smoothing);
      //  }

      //  AddForce(_totalForce);

      //  _prevSteering = _steeringInput;
      //} else {
      //  vehicleController.steering.useRawInput = false;
      //  _steeringWheelConnected = false;
      //}
    }

    void Start() {
      if (TryGetComponent<VehicleStandardInput>(out var comp)) {
        input = comp;
      } else {
        Debug.LogWarning("VehicleStandardInput component not found in attached game object. Steering wheel input will not work.");
      }
      InitializeLogitech();
      InitializeForceFeedbackSettings();
    }

    void Update() {
      if (input != null) {
        return;
      }
      if (!WheelIsConnected) {
        return;
      }
      UpdateWheelSettings();
    }

    private bool WheelIsConnected {
      get { return LogitechGSDK.LogiIsConnected(0); }
    }

    void AddForce(float force) {
      LogitechGSDK.LogiPlayConstantForce(0, (int)force);
    }

    void ResetForce() {
      LogitechGSDK.LogiStopConstantForce(0);
    }

    void OnApplicationQuit() {
      LogitechGSDK.LogiSteeringShutdown();
    }

    private void OnDestroy() {
      ResetInputsAndForces();
    }
  }
}
