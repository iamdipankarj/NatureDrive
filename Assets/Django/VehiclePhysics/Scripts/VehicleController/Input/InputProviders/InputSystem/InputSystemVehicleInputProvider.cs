using UnityEngine;
using UnityEngine.InputSystem;

#if UNITY_EDITOR
using UnityEditor;
using Django.NUI;
#endif

namespace Django.VehiclePhysics.Input {
  /// <summary>
  ///     Class for handling input through new InputSystem
  /// </summary>
  public partial class InputSystemVehicleInputProvider : VehicleInputProviderBase {
    /// <summary>
    ///     Number of H-shifter gears. Apart from changing this value when adding gears, also add/remove the events in the
    ///     Awake() method
    ///     to match the change.
    /// </summary>
    private const int H_SHIFTER_GEAR_COUNT = 10;

    public static SolaceInputActions vehicleInputActions;

    /// <summary>
    ///     Should mouse be used for input?
    /// </summary>
    [Tooltip("Should mouse be used for input?")]
    public bool mouseInput;

    private readonly bool[] _shiftIntoHeld = new bool[H_SHIFTER_GEAR_COUNT];
    private float _throttle;
    private float _brakes;
    private float _steering;
    private float _clutch;
    private float _handbrake;
    private bool _horn;
    private bool _boost;

    public new void Awake() {
      base.Awake();

      vehicleInputActions = new SolaceInputActions();
      vehicleInputActions.Enable();

      // Gear shift inputs.
      SetupGearShiftInput(vehicleInputActions.Car.ShiftIntoR1, 0);
      SetupGearShiftInput(vehicleInputActions.Car.ShiftInto0, 1);
      SetupGearShiftInput(vehicleInputActions.Car.ShiftInto1, 2);
      SetupGearShiftInput(vehicleInputActions.Car.ShiftInto2, 3);
      SetupGearShiftInput(vehicleInputActions.Car.ShiftInto3, 4);
      SetupGearShiftInput(vehicleInputActions.Car.ShiftInto4, 5);
      SetupGearShiftInput(vehicleInputActions.Car.ShiftInto5, 6);
      SetupGearShiftInput(vehicleInputActions.Car.ShiftInto6, 7);
      SetupGearShiftInput(vehicleInputActions.Car.ShiftInto7, 8);
      SetupGearShiftInput(vehicleInputActions.Car.ShiftInto8, 9);

      vehicleInputActions.Car.Horn.started += ctx => _horn = true;
      vehicleInputActions.Car.Horn.canceled += ctx => _horn = false;

      vehicleInputActions.Car.Boost.started += ctx => _boost = true;
      vehicleInputActions.Car.Boost.canceled += ctx => _boost = false;
    }

    private void SetupGearShiftInput(InputAction gearShiftAction, int index) {
      gearShiftAction.started += ctx => _shiftIntoHeld[index] = true;
      gearShiftAction.canceled += ctx => _shiftIntoHeld[index] = false;
    }

    public void Update() {
      _throttle = mouseInput
                      ? Mathf.Clamp(GetMouseVertical(), 0f, 1f)
                      : vehicleInputActions.Car.Throttle.ReadValue<float>();
      _brakes = mouseInput
                    ? -Mathf.Clamp(GetMouseVertical(), -1f, 0f)
                    : vehicleInputActions.Car.Brakes.ReadValue<float>();
      _steering = mouseInput
                      ? Mathf.Clamp(GetMouseHorizontal(), -1f, 1f)
                      : vehicleInputActions.Car.Steer.ReadValue<float>();
      _clutch = vehicleInputActions.Car.Clutch.ReadValue<float>();
      _handbrake = vehicleInputActions.Car.HandBrake.ReadValue<float>();
    }


    public void OnEnable() {
      vehicleInputActions?.Enable();
    }


    public void OnDisable() {
      vehicleInputActions?.Disable();
    }


    public override float Throttle() {
      return _throttle;
    }


    public override float Brakes() {
      return _brakes;
    }


    public override float Steering() {
      return _steering;
    }


    public override float Clutch() {
      return _clutch;
    }


    public override float Handbrake() {
      return _handbrake;
    }


    public override bool EngineStartStop() {
      return vehicleInputActions.Car.EngineStartStop.triggered;
    }


    public override bool ExtraLights() {
      return vehicleInputActions.Car.ExtraLights.triggered;
    }


    public override bool HighBeamLights() {
      return vehicleInputActions.Car.HighBeamLights.triggered;
    }


    public override bool HazardLights() {
      return vehicleInputActions.Car.HazardLights.triggered;
    }


    public override bool Horn() {
      return _horn;
    }


    public override bool LeftBlinker() {
      return vehicleInputActions.Car.LeftBlinker.triggered;
    }


    public override bool LowBeamLights() {
      return vehicleInputActions.Car.LowBeamLights.triggered;
    }


    public override bool RightBlinker() {
      return vehicleInputActions.Car.RightBlinker.triggered;
    }


    public override bool ShiftDown() {
      return vehicleInputActions.Car.ShiftDown.triggered;
    }


    public override bool ShiftUp() {
      return vehicleInputActions.Car.ShiftUp.triggered;
    }


    public override void OnDestroy() {
      base.OnDestroy();

      _throttle = 0;
      _brakes = 0;
      _steering = 0;
      _clutch = 0;
      _handbrake = 0;
    }


    /// <summary>
    ///     Used for H-shifters and direct shifting into gear on non-sequential gearboxes.
    /// </summary>
    public override int ShiftInto() {
      for (int i = 0; i < H_SHIFTER_GEAR_COUNT; i++) {
        if (_shiftIntoHeld[i]) {
          return i - 1;
        }
      }

      return -999;
    }


    public override bool TrailerAttachDetach() {
      return vehicleInputActions.Car.TrailerAttachDetach.triggered;
    }


    public override bool FlipOver() {
      return vehicleInputActions.Car.FlipOver.triggered;
    }


    public override bool Boost() {
      return _boost;
    }


    public override bool CruiseControl() {
      return vehicleInputActions.Car.CruiseControl.triggered;
    }


    private float GetMouseHorizontal() {
      Vector2 mousePos = Mouse.current.position.ReadValue();
      float percent = Mathf.Clamp(mousePos.x / Screen.width, -1f, 1f);
      if (percent < 0.5f) {
        return -(0.5f - percent) * 2.0f;
      }

      return (percent - 0.5f) * 2.0f;
    }


    private float GetMouseVertical() {
      Vector2 mousePos = Mouse.current.position.ReadValue();
      float percent = Mathf.Clamp(mousePos.y / Screen.height, -1f, 1f);
      if (percent < 0.5f) {
        return -(0.5f - percent) * 2.0f;
      }

      return (percent - 0.5f) * 2.0f;
    }
  }
}


#if UNITY_EDITOR

namespace Django.VehiclePhysics.Input {
  [CustomEditor(typeof(InputSystemVehicleInputProvider))]
  public partial class InputSystemVehicleInputProviderEditor : NVP_NUIEditor {
    public override bool OnInspectorNUI() {
      if (!base.OnInspectorNUI()) {
        return false;
      }

      drawer.Info(
          "Input settings for Unity's new input system can be changed by modifying 'VehicleInputActions' " +
          "file (double click on it to open).");
      drawer.Field("mouseInput");

      drawer.EndEditor(this);
      return true;
    }


    public override bool UseDefaultMargins() {
      return false;
    }
  }
}

#endif
