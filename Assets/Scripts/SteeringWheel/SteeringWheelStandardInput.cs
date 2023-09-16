using EVP;
using System;
using UnityEngine;

namespace Solace {
  public abstract class SteeringWheelStandardInput : MonoBehaviour {
    private bool[] buttonDown = new bool[128];
    private bool[] buttonPressed = new bool[128];
    private bool[] buttonWasPressed = new bool[128];

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
    /// Should all steering filtering and smoothing be ignored?
    /// </summary>
    public bool useDirectInput = true;

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
    private int axisResolution = 65534;
    private float minAxisResolution = -32767;
    private float maxAxisResolution = 32767;

    private bool flipSteeringInput = false;
    private bool flipThrottleInput = false;
    private bool flipBrakeInput = false;
    private bool flipClutchInput = false;
    private bool flipHandbrakeInput = false;

    public Axis steeringAxis = Axis.XPosition;
    public Axis throttleAxis = Axis.YPosition;
    public Axis brakeAxis = Axis.ZRotation;
    public Axis clutchAxis = Axis.ZPosition;
    public Axis handbrakeAxis = Axis.None;

    // Paddle Shift
    private int shiftUpButton = 4;
    private int shiftDownButton = 5;
    private int altShiftUpButton = 4;
    private int altShiftDownButton = 5;

    // Acc Buttons
    private int squareButton = 1;

    // Gears
    private int shiftIntoNeutralButton = -1;
    private int shiftIntoReverseButton = 18;
    private int shiftInto1stButton = 13;
    private int shiftInto2ndButton = 14;
    private int shiftInto3rdButton = 15;
    private int shiftInto4thButton = 16;
    private int shiftInto5thButton = 17;
    private int shiftInto6thButton = 18;
    private int handbrakeButton = 0;

    // Inputs
    [NonSerialized]
    public float steeringInput;
    [NonSerialized]
    public float throttleInput;
    [NonSerialized]
    public float brakeInput;
    [NonSerialized]
    public float clutchInput;
    [NonSerialized]
    public float handbrakeInput = 0;

    [NonSerialized]
    public int shiftIntoInput = -999;
    [NonSerialized]
    public bool shiftUpInput = false;
    [NonSerialized]
    public bool shiftDownInput = false;
    [NonSerialized]
    public bool squareInput;

    // Steering forces from vehicle controller
    private float _centerPosition;
    private bool _steeringWheelConnected;
    private float _prevSteering;
    private float _steerVelocity;
    private ForceFeedbackSettings _ffbSettings;
    private LogitechGSDK.LogiControllerPropertiesData _properties;
    private LogitechGSDK.DIJOYSTATE2ENGINES _wheelInput;

    private void ResetInputsAndForces() {
      steeringInput = 0;
      throttleInput = 0;
      brakeInput = 0;
      clutchInput = 0;
      handbrakeInput = 0;
      shiftIntoInput = -999;
      shiftUpInput = false;
      shiftDownInput = false;
    }

    /// <summary>
    /// Is the steering wheel currently connected? Read only.
    /// </summary>
    public bool SteeringWheelConnected {
      get { return _steeringWheelConnected; }
    }

    void SetVehicleInputs() {
      // Shift Up
      shiftUpInput = GetButtonDown(shiftUpButton) || GetButtonDown(altShiftUpButton);

      // Shift Down
      shiftDownInput = GetButtonDown(shiftDownButton) || GetButtonDown(altShiftDownButton);

      shiftIntoInput = -999;

      if (GetButtonDown(squareButton)) {
        Debug.Log("Will switch camera");
      }

      // H-shifter
      if (GetButtonPressed(shiftIntoReverseButton)) {
        shiftIntoInput = -1;
      } else if (GetButtonPressed(shiftIntoNeutralButton)) {
        shiftIntoInput = 0;
      } else if (GetButtonPressed(shiftInto1stButton)) {
        shiftIntoInput = 1;
      } else if (GetButtonPressed(shiftInto2ndButton)) {
        shiftIntoInput = 2;
      } else if (GetButtonPressed(shiftInto3rdButton)) {
        shiftIntoInput = 3;
      } else if (GetButtonPressed(shiftInto4thButton)) {
        shiftIntoInput = 4;
      } else if (GetButtonPressed(shiftInto5thButton)) {
        shiftIntoInput = 5;
      } else if (GetButtonPressed(shiftInto6thButton)) {
        shiftIntoInput = 6;
      }

      // Handbrake
      if (handbrakeAxis != Axis.None) {
        handbrakeInput = GetAxisValue(handbrakeAxis, _wheelInput, true);
        if (flipHandbrakeInput) handbrakeInput = -handbrakeInput;
      } else {
        handbrakeInput = GetButtonPressed(handbrakeButton) ? 1f : 0f;
      }
    }

    void GetWheelInputs() {
      _wheelInput = LogitechGSDK.LogiGetStateUnity(0);

      // Steer angle
      steeringInput = GetAxisValue(steeringAxis, _wheelInput, false) * steeringSensitivity;
      if (flipSteeringInput) steeringInput = -steeringInput;
      float steerDelta = steeringInput - _prevSteering;
      _steerVelocity = steerDelta / Time.deltaTime;

      // Throttle
      throttleInput = GetAxisValue(throttleAxis, _wheelInput, true);
      if (flipThrottleInput) throttleInput = -throttleInput;

      // Brake
      brakeInput = GetAxisValue(brakeAxis, _wheelInput, true);
      if (flipBrakeInput) brakeInput = -brakeInput;

      // Clutch
      clutchInput = GetAxisValue(clutchAxis, _wheelInput, true);
      if (flipClutchInput) clutchInput = -clutchInput;

      // Buttons
      for (int i = 0; i < 128; i++) {
        buttonWasPressed[i] = buttonPressed[i];
        buttonPressed[i] = _wheelInput.rgbButtons[i] == 128;
        buttonDown[i] = !buttonWasPressed[i] && buttonPressed[i];
      }
    }

    bool GetButtonPressed(int buttonIndex) {
      if (buttonIndex < 0) {
        return false;
      }
      return buttonPressed[buttonIndex];
    }

    bool GetButtonDown(int buttonIndex) {
      if (buttonIndex < 0) {
        return false;
      }
      return buttonDown[buttonIndex];
    }

    private float GetAxisValue(Axis axis, LogitechGSDK.DIJOYSTATE2ENGINES wheelState, bool zeroToOne, bool debug = false) {
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
        float finalValue = 1 - ((rawValue - minAxisResolution) / (maxAxisResolution - minAxisResolution));
        if (debug) {
          Debug.Log(finalValue);
        }
        return finalValue;
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

    protected void SW_Initialize() {
      LogitechGSDK.LogiSteeringInitialize(false);
      UpdateWheelSettings();
    }

    #region Force Feedback

    [SerializeField][Range(0, 100)] private float _lowSpeedFrictionForce;
    [SerializeField][Range(0, 100)] private float _totalForce = 0;
    [SerializeField][Range(0, 100)] private float _satForce;
    [SerializeField][Range(0, 100)] private float _frictionForce;
    [SerializeField][Range(0, 100)] private float _centeringForce;

    // Vehicle-specific coefficients
    private float _overallCoeff = 1f;
    private float _frictionCoeff = 1f;
    private float _lowSpeedFrictionCoeff = 1f;
    private float _satCoeff = 1f;
    private float _centeringCoeff = 1f;
    private float _totalForceVelocity;

    private void SW_FFB(SteeringWheelVehicleData data) {
      float newTotalForce = 0;

      // Self Aligning Torque
      float leftFactor = data.leftWheelLoad/ 12000f * data.leftWheelFrictionPresetZ;
      float rightFactor = data.rightWheelLoad / 12000f * data.rightWheelFrictionPresetZ;
      float combinedFactor = leftFactor + rightFactor;
      float totalSlip = data.leftWheelLateralSlip * leftFactor + data.rightWheelLongitudinalSlip * rightFactor;
      float absSlip = totalSlip < 0 ? -totalSlip : totalSlip;
      float slipSign = totalSlip < 0 ? -1f : 1f;
      _satForce = slipSATCurve.Evaluate(absSlip * slipMultiplier) * -slipSign * maxSatForce * combinedFactor * _satCoeff;
      newTotalForce += Mathf.Lerp(0f, _satForce, data.vehicleSpeed - 0.4f);

      // Determine target center  position (changes with spring compression)
      _centerPosition = ((data.rightWheelSpringLength / data.rightWheelSpringMaxLength) - (data.leftWheelSpringLength / data.leftWheelSpringMaxLength)) * centerPositionDrift;

      // Calculate centering force
      _centeringForce = (steeringInput - _centerPosition) * centeringForceStrength * _centeringCoeff;
      newTotalForce += _centeringForce;

      // Low speed friction
      _lowSpeedFrictionForce = Mathf.Lerp(lowSpeedFriction, 0, data.vehicleSpeed - 0.2f) * _lowSpeedFrictionCoeff;

      // Friction 
      _frictionForce = friction * _frictionCoeff;

      // Apply friction
      LogitechGSDK.LogiPlayDamperForce(0, (int)(_lowSpeedFrictionForce + _frictionForce));

      newTotalForce *= overallEffectStrength * _overallCoeff;
      if (smoothing < 0.001f) {
        _totalForce = newTotalForce;
      } else {
        _totalForce = Mathf.SmoothDamp(_totalForce, newTotalForce, ref _totalForceVelocity, smoothing);
      }

      AddForce(_totalForce);

      _prevSteering = steeringInput;
    }

    #endregion

    public void PlayCollision(int strength) {
      LogitechGSDK.LogiPlayFrontalCollisionForce(0, strength);
    }

    void AddForce(float force) {
      LogitechGSDK.LogiPlayConstantForce(0, (int)force);
    }

    void ResetForce() {
      LogitechGSDK.LogiStopConstantForce(0);
    }

    protected void SW_Update() {
      if (IsDeviceConnected) {
        UpdateWheelSettings();
        GetWheelInputs();
        SetVehicleInputs();
      }
    }

    protected void SW_FixedUpdate(SteeringWheelVehicleData data) {
      if (IsDeviceConnected) {
        SW_FFB(data);
      }
    }

    private bool IsDeviceConnected {
      get {
        return LogitechGSDK.LogiUpdate() && LogitechGSDK.LogiIsConnected(0);
      }
    }

    void OnApplicationQuit() {
#if UNITY_EDITOR
      Debug.Log("Steering Wheel Shutting down");
#endif
      LogitechGSDK.LogiSteeringShutdown();
    }

    private void OnDestroy() {
      ResetInputsAndForces();
    }
  }
}
