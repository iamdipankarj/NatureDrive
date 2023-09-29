using Rewired;
using UnityEngine;

namespace Solace {
  public class SteeringWheelTest : MonoBehaviour {
    public Transform steeringWheel;
    public Transform throttlePad;
    public Transform breakPad;
    public Transform clutchPad;

    private Player player;

    /// <summary>
    /// Ideal values are 450 and 1 for 100% realistic simulation
    /// wheel turn ratio to be modified only for sim racing
    /// </summary>
    private int wheelRotationRange = 450;
    private float steeringWheelTurnRatio = 1f;

    private Vector3 _initialSteeringWheelRotation;

    private Vector3 _initialThrottleRotation;
    private Vector3 _initialBreakRotation;
    private Vector3 _initialClutchRotation;

    private float maxPadAngle = 40f;

    private int currentGear = 0;
    private int MAX_GEAR = 8;
    private int MIN_GEAR = -1;

    private void Start() {
      player = ReInput.players.GetPlayer(0);
      _initialSteeringWheelRotation = steeringWheel.localRotation.eulerAngles;

      _initialThrottleRotation = throttlePad.localRotation.eulerAngles;
      _initialBreakRotation = throttlePad.localRotation.eulerAngles;
      _initialClutchRotation = clutchPad.localRotation.eulerAngles;
    }

    // Update is called once per frame
    void Update() {
      float throttleAxis = player.GetAxis(RewiredUtils.Throttle);
      float breakAxis = player.GetAxis(RewiredUtils.Brake);
      float clutchAxis = player.GetAxis(RewiredUtils.Clutch);

      // Throttle
      throttlePad.localRotation = Quaternion.Euler(_initialThrottleRotation);
      throttlePad.Rotate(Vector3.left, -throttleAxis * maxPadAngle);

      // Break
      breakPad.localRotation = Quaternion.Euler(_initialBreakRotation);
      breakPad.Rotate(Vector3.left, -breakAxis * maxPadAngle);

      // Clutch
      clutchPad.localRotation = Quaternion.Euler(_initialClutchRotation);
      clutchPad.Rotate(Vector3.left, -clutchAxis * maxPadAngle);

      // Paddle Shift
      if (player.GetButtonDown(RewiredUtils.ShiftUp)) {
        if (currentGear < MAX_GEAR) {
          currentGear++;
        }
      } else if (player.GetButtonDown(RewiredUtils.ShiftDown)) {
        if (currentGear > MIN_GEAR) {
          currentGear--;
        }
      }

      if (player.GetButton(RewiredUtils.ShiftInto0)) {
        currentGear = 0;
      } else if (player.GetButton(RewiredUtils.ShiftInto1)) {
        currentGear = 1;
      } else if (player.GetButton(RewiredUtils.ShiftInto2)) {
        currentGear = 2;
      } else if (player.GetButton(RewiredUtils.ShiftInto3)) {
        currentGear = 3;
      } else if (player.GetButton(RewiredUtils.ShiftInto4)) {
        currentGear = 4;
      } else if (player.GetButton(RewiredUtils.ShiftInto5)) {
        currentGear = 5;
      } else if (player.GetButton(RewiredUtils.ShiftInto6)) {
        currentGear = 6;
      } else if (player.GetButton(RewiredUtils.ShiftInto7)) {
        currentGear = 7;
      } else if (player.GetButton(RewiredUtils.ShiftInto8)) {
        currentGear = 8;
      } else if (player.GetButton(RewiredUtils.ShiftIntoR1)) {
        currentGear = -1;
      } else {
        currentGear = 0;
      }

      Debug.Log($"Current Gear: {currentGear}");

      float steerAxis = player.GetAxis(RewiredUtils.Steering);
      float wheelAngle = steerAxis * steeringWheelTurnRatio * wheelRotationRange;
      steeringWheel.localRotation = Quaternion.Euler(_initialSteeringWheelRotation);
      steeringWheel.Rotate(Vector3.forward, -wheelAngle);
    }
  }
}
