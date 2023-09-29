using System;
using UnityEngine;
using Rewired;
using Solace;

namespace NWH.VehiclePhysics2.Input {
  /// <summary>
  ///     Class for handling vehicle input through Rewired.
  /// </summary>
  public class RewiredVehicleInputProvider2 : VehicleInputProviderBase {
    /// <summary>
    ///     Number of H-shifter gears. Apart from changing this value when adding gears, also add/remove the events in the
    ///     Awake() method
    ///     to match the change.
    /// </summary>
    private const int GearCount = 10;

    private Player player;

    private readonly bool[] _shiftIntoHeld = new bool[GearCount];

    /// <summary>
    ///     Names of input bindings for each individual gears. If you need to add more gears modify this and the corresponding
    ///     iterator in the
    ///     ShiftInto() function.
    /// </summary>
    [NonSerialized]
    [Tooltip("Names of input bindings for each individual gears. If you need to add more gears modify this and the corresponding\r\niterator in the\r\nShiftInto() function.")]
    public string[] shiftInputNames = {
      RewiredUtils.ShiftIntoR1,
      RewiredUtils.ShiftInto0,
      RewiredUtils.ShiftInto1,
      RewiredUtils.ShiftInto2,
      RewiredUtils.ShiftInto3,
      RewiredUtils.ShiftInto4,
      RewiredUtils.ShiftInto5,
      RewiredUtils.ShiftInto6,
      RewiredUtils.ShiftInto7,
      RewiredUtils.ShiftInto8
    };

    private float _throttle;
    private float _brakes;
    private float _steering;
    private float _clutch;
    private float _handbrake;

    public override void Awake() {
      base.Awake();
      player = ReInput.players.GetPlayer(0);
    }

    public void Update() {
      _throttle = player.GetAxis(RewiredUtils.Throttle);
      _brakes = player.GetAxis(RewiredUtils.Brake);
      _steering = player.GetAxis(RewiredUtils.Steering);
      _clutch = player.GetAxis(RewiredUtils.Clutch);
      _handbrake = player.GetAxis(RewiredUtils.HandBrake);
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
      return player.GetButtonDown(RewiredUtils.EngineStartStop);
    }

    public override bool ExtraLights() {
      return player.GetButtonDown(RewiredUtils.ExtraLights);
    }

    public override bool HighBeamLights() {
      return player.GetButtonDown(RewiredUtils.HighBeamLights);
    }

    public override bool HazardLights() {
      return player.GetButtonDown(RewiredUtils.HazardLights);
    }

    public override bool Horn() {
      return player.GetButton(RewiredUtils.Horn);
    }

    public override bool LeftBlinker() {
      return player.GetButtonDown(RewiredUtils.LeftBlinker);
    }

    public override bool LowBeamLights() {
      return player.GetButtonDown(RewiredUtils.LowBeamLights);
    }

    public override bool RightBlinker() {
      return player.GetButtonDown(RewiredUtils.RightBlinker);
    }

    public override bool ShiftDown() {
      return player.GetButtonDown(RewiredUtils.ShiftDown);
    }

    public override bool ShiftUp() {
      return player.GetButtonDown(RewiredUtils.ShiftUp);
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
      for (int i = -1; i < 9; i++) {
        if (player.GetButton(shiftInputNames[i + 1])) {
          return i;
        }
      }
      return -999;
    }

    public override bool TrailerAttachDetach() {
      return player.GetButtonDown(RewiredUtils.TrailerAttachDetach);
    }

    public override bool FlipOver() {
      return player.GetButtonDown(RewiredUtils.FlipOver);
    }

    public override bool Boost() {
      return player.GetButton(RewiredUtils.Boost);
    }

    public override bool CruiseControl() {
      return player.GetButtonDown(RewiredUtils.CruiseControl);
    }
  }
}