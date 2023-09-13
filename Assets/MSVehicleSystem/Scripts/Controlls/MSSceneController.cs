using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;
using System;

namespace MSVehicle {
  [Serializable]
  public class Controls {
    [Header("Gears")]
    public KeyCode manualOrAutoGears = KeyCode.O;
    public KeyCode increasedGear = KeyCode.LeftShift;

    [Space(15)]
    public KeyCode decreasedGear = KeyCode.LeftControl;

    [Space(10)]
    [Header("Lights")]
    public KeyCode flashesRightAlert = KeyCode.E;
    public KeyCode flashesLeftAlert = KeyCode.Q;
    public KeyCode mainLightsInput = KeyCode.L;
    public KeyCode extraLightsInput = KeyCode.K;

    [Space(15)]
    public KeyCode headlightsInput = KeyCode.J;
    public KeyCode warningLightsInput = KeyCode.H;

    [Space(10)]
    [Header("Vehicle")]
    [Tooltip("The input that must be pressed to turn the vehicle engine on or off.")]
    public KeyCode startTheVehicle = KeyCode.F;

    [Space(15)]
    [Tooltip("The input that must be pressed to enter or exit the vehicle.")]
    public KeyCode enterEndExit = KeyCode.T;

    [Space(15)]
    [Tooltip("The input that must be pressed to emit the horn sound of the vehicle.")]
    public KeyCode hornInput = KeyCode.B;

    [Space(15)]
    [Tooltip("The input that must be pressed to activate or deactivate the vehicle hand brake.")]
    public KeyCode handBrakeInput = KeyCode.Space;

    [Space(15)]
    [Tooltip("The input that must be pressed to toggle between the cameras of the vehicle.")]
    public KeyCode switchingCameras = KeyCode.C;

    [Space(15)]
    [Tooltip("The input that must be pressed to change the height of the vehicle.")]
    public KeyCode changeSuspensionHeight = KeyCode.I;

    [Space(15)]
    [Tooltip("The input that must be pressed to activate nitro mode of the vehicle.")]
    public KeyCode nitro = KeyCode.G;
  }

  public class MSSceneController : MonoBehaviour {

    [Space(10)]
    [Header("*CONTROLS")]
    #region defineInputs
    [Tooltip("Vertical input recognized by the system")]
    public string _verticalInput = "Vertical";

    [Tooltip("Horizontal input recognized by the system")]
    public string _horizontalInput = "Horizontal";

    [Tooltip("Horizontal input for camera movements")]
    public string _mouseXInput = "Mouse X";

    [Tooltip("Vertical input for camera movements")]
    public string _mouseYInput = "Mouse Y";

    [Tooltip("Scroll input, to zoom in and out of the cameras.")]
    public string _mouseScrollWheelInput = "Mouse ScrollWheel";
    #endregion

    [Tooltip("Here you can configure the vehicle controls, choose the desired inputs and also, deactivate the unwanted ones.")]
    public Controls controls;

    #region customizeInputs
    [HideInInspector]
    public float verticalInput = 0;
    [HideInInspector]
    public float horizontalInput = 0;
    [HideInInspector]
    public float mouseXInput = 0;
    [HideInInspector]
    public float mouseYInput = 0;
    [HideInInspector]
    public float mouseScrollWheelInput = 0;
    #endregion

    private MSVehicleController vehicleCode;

    void Start() {
      vehicleCode = GetComponent<MSVehicleController>();
      vehicleCode.theEngineIsRunning = false;
      vehicleCode._vehicleState = MSVehicleController.ControlState.isNull;
      vehicleCode.EnterInVehicle(true); //vehicle state >> isPlayer
      vehicleCode.theEngineIsRunning = vehicleCode._vehicleSettings.startOn;
    }

    #region UPDATE REGION
    void SetUpFirstVehicleOnRunTime() {
      vehicleCode.EnterInVehicle(true); //vehicle state >> isPlayer
      vehicleCode.theEngineIsRunning = vehicleCode._vehicleSettings.startOn;
    }

    void Update() {
      verticalInput = Mathf.Clamp(Input.GetAxis(_verticalInput), -1, 1);
      horizontalInput = Mathf.Clamp(Input.GetAxis(_horizontalInput), -1, 1);
      mouseXInput = Input.GetAxis(_mouseXInput);
      mouseYInput = Input.GetAxis(_mouseYInput);
      mouseScrollWheelInput = Input.GetAxis(_mouseScrollWheelInput);

      //set all Player Inputs on Vehicle code
      SetCurrentVehicleInputs();
      SetUpFirstVehicleOnRunTime();
    }

    void SetCurrentVehicleInputs() {
      if (vehicleCode) {
        if (vehicleCode._vehicleState == MSVehicleController.ControlState.isPlayer) {
          vehicleCode.verticalInput = verticalInput;
          vehicleCode.horizontalInput = horizontalInput;
          vehicleCode.mouseXInput = mouseXInput;
          vehicleCode.mouseYInput = mouseYInput;
          vehicleCode.mouseScrollWheelInput = mouseScrollWheelInput;
          //gears
          //input manual or automatic gears
          if (Input.GetKeyDown(controls.manualOrAutoGears)) {
            vehicleCode.automaticGears = !vehicleCode.automaticGears;
          }

          //gears logic
          if (vehicleCode.automaticGears) {
            if (Input.GetKey(controls.handBrakeInput)) {
              vehicleCode.handBrakeTrue = true;
            } else {
              vehicleCode.handBrakeTrue = false;
            }
          } else {
            if (Input.GetKeyDown(controls.handBrakeInput)) {
              vehicleCode.handBrakeTrue = !vehicleCode.handBrakeTrue;
            }
          }

          //up and down gear
          if (!vehicleCode.automaticGears) {
            if (Input.GetKeyDown(controls.increasedGear) && vehicleCode.currentGear < vehicleCode._vehicleTorque.numberOfGears && !vehicleCode.changinGears) {
              vehicleCode.StartCoroutine(nameof(vehicleCode.ChangeGears), vehicleCode.currentGear + 1);
            }
            if (Input.GetKeyDown(controls.decreasedGear) && vehicleCode.currentGear > -1 && !vehicleCode.changinGears) {
              vehicleCode.StartCoroutine(nameof(vehicleCode.ChangeGears), vehicleCode.currentGear - 1);
            }
          }

          //horn input
          if (Input.GetKeyDown(controls.hornInput)) {
            vehicleCode.hornIsOn = true;
          }

          //turn on and turn off input
          if (vehicleCode.youCanCall) {
            if (vehicleCode.theEngineIsRunning) {
              if (Input.GetKeyDown(controls.startTheVehicle)) {
                vehicleCode.StartCoroutine(nameof(vehicleCode.StartEngineCoroutine), false);
              }
            } else {
              if (Input.GetKeyDown(controls.startTheVehicle)) {
                if (vehicleCode.currentFuelLiters > 0) {
                  vehicleCode.enableEngineSound = true;
                  if (vehicleCode._sounds.engineSound) {
                    if (vehicleCode.engineSoundAUD) {
                      vehicleCode.engineSoundAUD.pitch = 0.5f;
                      vehicleCode.pitchAUDforRPM = 0.7f;
                    }
                  }
                  if (vehicleCode._sounds.engineStartSound) {
                    if (vehicleCode.engineStartSoundAUD) {
                      vehicleCode.previousDelayStartEngine = vehicleCode._vehicleSettings.delayToStartTheEngine;
                      vehicleCode._vehicleSettings.delayToStartTheEngine = Mathf.Abs(vehicleCode._sounds.engineStartSound.length - 0.1f);
                      vehicleCode.engineSoundAUD.volume = 0;
                      vehicleCode.engineStartSoundAUD.PlayOneShot(vehicleCode.engineStartSoundAUD.clip);
                    }
                  }
                  //
                  vehicleCode.StartCoroutine(nameof(vehicleCode.StartEngineCoroutine), true);
                }
              }
            }
          }

          if (vehicleCode._suspension.vehicleCustomHeights.Length > 0) {
            if (Input.GetKeyDown(controls.changeSuspensionHeight)) {
              if (vehicleCode._suspension.indexCustomSuspensionHeight < (vehicleCode._suspension.vehicleCustomHeights.Length - 1)) {
                vehicleCode._suspension.indexCustomSuspensionHeight++;
              } else if (vehicleCode._suspension.indexCustomSuspensionHeight >= (vehicleCode._suspension.vehicleCustomHeights.Length - 1)) {
                vehicleCode._suspension.indexCustomSuspensionHeight = 0;
              }
            }
          }

          //NITRO input
          if (vehicleCode._additionalFeatures.useNitro) {
            if (Input.GetKeyDown(controls.nitro)) {
              vehicleCode._additionalFeatures.nitroIsTrueInput = !vehicleCode._additionalFeatures.nitroIsTrueInput;
            }
          }

          //LIGHTS
          //main lights
          if (Input.GetKeyDown(controls.mainLightsInput)) {
            if (!vehicleCode.lowLightOn && !vehicleCode.highLightOn) {
              vehicleCode.lowLightOn = true;
              vehicleCode.brakeLightsIntensity = 0.5f;
            } else if (vehicleCode.lowLightOn && !vehicleCode.highLightOn) {
              vehicleCode.lowLightOn = false;
              vehicleCode.highLightOn = true;
              vehicleCode.brakeLightsIntensity = 0.5f;
            } else if (!vehicleCode.lowLightOn && vehicleCode.highLightOn) {
              vehicleCode.lowLightOn = false;
              vehicleCode.highLightOn = false;
              vehicleCode.brakeLightsIntensity = 0.0f;
            }
          }
          //head lights
          if (Input.GetKeyDown(controls.headlightsInput)) {
            vehicleCode.headlightsOn = !vehicleCode.headlightsOn;
          }
          //flashesRightAlert
          if (Input.GetKeyDown(controls.flashesRightAlert) && !vehicleCode.rightBlinkersOn && !vehicleCode.alertOn) {
            vehicleCode.rightBlinkersOn = true;
            vehicleCode.leftBlinkersOn = false;
            vehicleCode.disableBlinkers1 = true;
          } else if (Input.GetKeyDown(controls.flashesRightAlert) && vehicleCode.rightBlinkersOn && !vehicleCode.alertOn) {
            vehicleCode.rightBlinkersOn = false;
            vehicleCode.leftBlinkersOn = false;
            vehicleCode.disableBlinkers1 = false;
          }
          //flashesLeftAlert
          if (Input.GetKeyDown(controls.flashesLeftAlert) && !vehicleCode.leftBlinkersOn && !vehicleCode.alertOn) {
            vehicleCode.rightBlinkersOn = false;
            vehicleCode.leftBlinkersOn = true;
            vehicleCode.disableBlinkers1 = true;
          } else if (Input.GetKeyDown(controls.flashesLeftAlert) && vehicleCode.leftBlinkersOn && !vehicleCode.alertOn) {
            vehicleCode.rightBlinkersOn = false;
            vehicleCode.leftBlinkersOn = false;
            vehicleCode.disableBlinkers1 = false;
          }
          //alertOn
          if (Input.GetKeyDown(controls.warningLightsInput)) {
            if (vehicleCode.alertOn) {
              vehicleCode.alertOn = false;
              vehicleCode.rightBlinkersOn = vehicleCode.leftBlinkersOn = false;
            } else {
              vehicleCode.alertOn = true;
              vehicleCode.rightBlinkersOn = vehicleCode.leftBlinkersOn = true;
            }
          }
          //extraLightsOn
          if (Input.GetKeyDown(controls.extraLightsInput)) {
            vehicleCode.extraLightsOn = !vehicleCode.extraLightsOn;
          }
        }
      }
    }
  }
  #endregion
}
