using UnityEngine;
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

  public class MSVehicleInput : Solace.VehicleStandardInput {
    [Tooltip("Here you can configure the vehicle controls, choose the desired inputs and also, deactivate the unwanted ones.")]
    public Controls controls;

    private MSVehicleController vc;

    void Start() {
      vc = GetComponent<MSVehicleController>();
      vc.theEngineIsRunning = false;
      vc._vehicleState = MSVehicleController.ControlState.isNull;
      vc.EnterInVehicle(true); //vehicle state >> isPlayer
      vc.theEngineIsRunning = vc._vehicleSettings.startOn;
    }

    #region UPDATE REGION
    void SetUpFirstVehicleOnRunTime() {
      vc.EnterInVehicle(true); //vehicle state >> isPlayer
      vc.theEngineIsRunning = vc._vehicleSettings.startOn;
    }

    void Update() {
      SetCurrentVehicleInputs();
      SetUpFirstVehicleOnRunTime();
    }

    void SetCurrentVehicleInputs() {
      if (vc._vehicleState == MSVehicleController.ControlState.isPlayer) {
        vc.verticalInput = Mathf.Clamp(base.throttleInput - base.brakeInput, -1, 1);
        vc.horizontalInput = Mathf.Clamp(base.steerInput, -1, 1);
        //gears
        //input manual or automatic gears
        if (Input.GetKeyDown(controls.manualOrAutoGears)) {
          vc.automaticGears = !vc.automaticGears;
        }

        //gears logic
        if (vc.automaticGears) {
          if (base.handbrakeInput) {
            vc.handBrakeTrue = true;
          } else {
            vc.handBrakeTrue = false;
          }
        } else {
          if (base.handbrakeInput) {
            vc.handBrakeTrue = !vc.handBrakeTrue;
          }
        }

        //up and down gear
        if (!vc.automaticGears) {
          if (Input.GetKeyDown(controls.increasedGear) && vc.currentGear < vc._vehicleTorque.numberOfGears && !vc.changinGears) {
            vc.StartCoroutine(nameof(vc.ChangeGears), vc.currentGear + 1);
          }
          if (Input.GetKeyDown(controls.decreasedGear) && vc.currentGear > -1 && !vc.changinGears) {
            vc.StartCoroutine(nameof(vc.ChangeGears), vc.currentGear - 1);
          }
        }

        //horn input
        if (Input.GetKeyDown(controls.hornInput)) {
          vc.hornIsOn = true;
        }

        //turn on and turn off input
        if (vc.youCanCall) {
          if (vc.theEngineIsRunning) {
            if (Input.GetKeyDown(controls.startTheVehicle)) {
              vc.StartCoroutine(nameof(vc.StartEngineCoroutine), false);
            }
          } else {
            if (Input.GetKeyDown(controls.startTheVehicle)) {
              if (vc.currentFuelLiters > 0) {
                vc.enableEngineSound = true;
                if (vc._sounds.engineSound) {
                  if (vc.engineSoundAUD) {
                    vc.engineSoundAUD.pitch = 0.5f;
                    vc.pitchAUDforRPM = 0.7f;
                  }
                }
                if (vc._sounds.engineStartSound) {
                  if (vc.engineStartSoundAUD) {
                    vc.previousDelayStartEngine = vc._vehicleSettings.delayToStartTheEngine;
                    vc._vehicleSettings.delayToStartTheEngine = Mathf.Abs(vc._sounds.engineStartSound.length - 0.1f);
                    vc.engineSoundAUD.volume = 0;
                    vc.engineStartSoundAUD.PlayOneShot(vc.engineStartSoundAUD.clip);
                  }
                }
                //
                vc.StartCoroutine(nameof(vc.StartEngineCoroutine), true);
              }
            }
          }
        }

        if (vc._suspension.vehicleCustomHeights.Length > 0) {
          if (Input.GetKeyDown(controls.changeSuspensionHeight)) {
            if (vc._suspension.indexCustomSuspensionHeight < (vc._suspension.vehicleCustomHeights.Length - 1)) {
              vc._suspension.indexCustomSuspensionHeight++;
            } else if (vc._suspension.indexCustomSuspensionHeight >= (vc._suspension.vehicleCustomHeights.Length - 1)) {
              vc._suspension.indexCustomSuspensionHeight = 0;
            }
          }
        }

        //NITRO input
        if (vc._additionalFeatures.useNitro) {
          if (Input.GetKeyDown(controls.nitro)) {
            vc._additionalFeatures.nitroIsTrueInput = !vc._additionalFeatures.nitroIsTrueInput;
          }
        }

        //LIGHTS
        //main lights
        if (Input.GetKeyDown(controls.mainLightsInput)) {
          if (!vc.lowLightOn && !vc.highLightOn) {
            vc.lowLightOn = true;
            vc.brakeLightsIntensity = 0.5f;
          } else if (vc.lowLightOn && !vc.highLightOn) {
            vc.lowLightOn = false;
            vc.highLightOn = true;
            vc.brakeLightsIntensity = 0.5f;
          } else if (!vc.lowLightOn && vc.highLightOn) {
            vc.lowLightOn = false;
            vc.highLightOn = false;
            vc.brakeLightsIntensity = 0.0f;
          }
        }
        //head lights
        if (Input.GetKeyDown(controls.headlightsInput)) {
          vc.headlightsOn = !vc.headlightsOn;
        }
        //flashesRightAlert
        if (Input.GetKeyDown(controls.flashesRightAlert) && !vc.rightBlinkersOn && !vc.alertOn) {
          vc.rightBlinkersOn = true;
          vc.leftBlinkersOn = false;
          vc.disableBlinkers1 = true;
        } else if (Input.GetKeyDown(controls.flashesRightAlert) && vc.rightBlinkersOn && !vc.alertOn) {
          vc.rightBlinkersOn = false;
          vc.leftBlinkersOn = false;
          vc.disableBlinkers1 = false;
        }
        //flashesLeftAlert
        if (Input.GetKeyDown(controls.flashesLeftAlert) && !vc.leftBlinkersOn && !vc.alertOn) {
          vc.rightBlinkersOn = false;
          vc.leftBlinkersOn = true;
          vc.disableBlinkers1 = true;
        } else if (Input.GetKeyDown(controls.flashesLeftAlert) && vc.leftBlinkersOn && !vc.alertOn) {
          vc.rightBlinkersOn = false;
          vc.leftBlinkersOn = false;
          vc.disableBlinkers1 = false;
        }
        //alertOn
        if (Input.GetKeyDown(controls.warningLightsInput)) {
          if (vc.alertOn) {
            vc.alertOn = false;
            vc.rightBlinkersOn = vc.leftBlinkersOn = false;
          } else {
            vc.alertOn = true;
            vc.rightBlinkersOn = vc.leftBlinkersOn = true;
          }
        }
        //extraLightsOn
        if (Input.GetKeyDown(controls.extraLightsInput)) {
          vc.extraLightsOn = !vc.extraLightsOn;
        }
      }
    }
  }
  #endregion
}
