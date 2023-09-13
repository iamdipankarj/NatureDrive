﻿using System.Collections;
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
    [Tooltip("If this variable is true, the control for this variable will be activated.")]
    public bool enable_manualOrAutoGears_Input_key = true;
    [Tooltip("If this variable is true, the control for this variable will be enabled on the mobile buttons.")]
    public bool enable_manualOrAutoGears_Button_Mobile = true;
    [Tooltip("The input that must be pressed to leave the vehicle in manual gears or automatic gears.")]
    public KeyCode manualOrAutoGears = KeyCode.O;
    [Tooltip("In this variable it is possible to associate a 'UI Button' to execute the described command.")]
    public Button manualOrAutoGearsMobileButton;

    [Space(15)]
    [Tooltip("If this variable is true, the control for this variable will be activated.")]
    public bool enable_increasedGear_Input_key = true;
    [Tooltip("If this variable is true, the control for this variable will be enabled on the mobile buttons.")]
    public bool enable_increasedGear_Button_Mobile = true;
    [Tooltip("The input that must be pressed to increase the vehicle's current gear.")]
    public KeyCode increasedGear = KeyCode.LeftShift;
    [Tooltip("In this variable it is possible to associate a 'UI Button' to execute the described command.")]
    public Button increasedGearMobileButton;

    [Space(15)]
    [Tooltip("If this variable is true, the control for this variable will be activated.")]
    public bool enable_decreasedGear_Input_key = true;
    [Tooltip("If this variable is true, the control for this variable will be enabled on the mobile buttons.")]
    public bool enable_decreasedGear_Button_Mobile = true;
    [Tooltip("The input that must be pressed to decrease the vehicle's current gear.")]
    public KeyCode decreasedGear = KeyCode.LeftControl;
    [Tooltip("In this variable it is possible to associate a 'UI Button' to execute the described command.")]
    public Button decreasedGearMobileButton;

    [Space(10)]
    [Header("Lights")]
    [Tooltip("If this variable is true, the control for this variable will be activated.")]
    public bool enable_flashesRightAlert_Input_key = true;
    [Tooltip("If this variable is true, the control for this variable will be enabled on the mobile buttons.")]
    public bool enable_flashesRightAlert_Button_Mobile = true;
    [Tooltip("The input that must be pressed turns on or off the blinking lights on the right side of the vehicle.")]
    public KeyCode flashesRightAlert = KeyCode.E;
    [Tooltip("In this variable it is possible to associate a 'UI Button' to execute the described command.")]
    public Button flashesRightAlertMobileButton;

    [Space(15)]
    [Tooltip("If this variable is true, the control for this variable will be activated.")]
    public bool enable_flashesLeftAlert_Input_key = true;
    [Tooltip("If this variable is true, the control for this variable will be enabled on the mobile buttons.")]
    public bool enable_flashesLeftAlert_Button_Mobile = true;
    [Tooltip("The input that must be pressed turns on or off the blinking lights on the left side of the vehicle.")]
    public KeyCode flashesLeftAlert = KeyCode.Q;
    [Tooltip("In this variable it is possible to associate a 'UI Button' to execute the described command.")]
    public Button flashesLeftAlertMobileButton;

    [Space(15)]
    [Tooltip("If this variable is true, the control for this variable will be activated.")]
    public bool enable_mainLightsInput_Input_key = true;
    [Tooltip("If this variable is true, the control for this variable will be enabled on the mobile buttons.")]
    public bool enable_mainLightsInput_Button_Mobile = true;
    [Tooltip("The input that must be pressed to turn the vehicle main lights on or off.")]
    public KeyCode mainLightsInput = KeyCode.L;
    [Tooltip("In this variable it is possible to associate a 'UI Button' to execute the described command.")]
    public Button mainLightsMobileButton;

    [Space(15)]
    [Tooltip("If this variable is true, the control for this variable will be activated.")]
    public bool enable_extraLightsInput_Input_key = true;
    [Tooltip("If this variable is true, the control for this variable will be enabled on the mobile buttons.")]
    public bool enable_extraLightsInput_Button_Mobile = true;
    [Tooltip("The input that must be pressed to turn the vehicle extra lights on or off.")]
    public KeyCode extraLightsInput = KeyCode.K;
    [Tooltip("In this variable it is possible to associate a 'UI Button' to execute the described command.")]
    public Button extraLightsMobileButton;

    [Space(15)]
    [Tooltip("If this variable is true, the control for this variable will be activated.")]
    public bool enable_headlightsInput_Input_key = true;
    [Tooltip("If this variable is true, the control for this variable will be enabled on the mobile buttons.")]
    public bool enable_headlightsInput_Button_Mobile = true;
    [Tooltip("The input that must be pressed to turn the vehicle headlights on or off.")]
    public KeyCode headlightsInput = KeyCode.J;
    [Tooltip("In this variable it is possible to associate a 'UI Button' to execute the described command.")]
    public Button headlightsMobileButton;

    [Space(15)]
    [Tooltip("If this variable is true, the control for this variable will be activated.")]
    public bool enable_warningLightsInput_Input_key = true;
    [Tooltip("If this variable is true, the control for this variable will be enabled on the mobile buttons.")]
    public bool enable_warningLightsInput_Button_Mobile = true;
    [Tooltip("The input that must be pressed to turn the vehicle warning lights on or off.")]
    public KeyCode warningLightsInput = KeyCode.H;
    [Tooltip("In this variable it is possible to associate a 'UI Button' to execute the described command.")]
    public Button warningLightsMobileButton;

    [Space(10)]
    [Header("Game")]
    [Tooltip("If this variable is true, the control for this variable will be activated.")]
    public bool enable_reloadScene_Input_key = true;
    [Tooltip("If this variable is true, the control for this variable will be enabled on the mobile buttons.")]
    public bool enable_reloadScene_Button_Mobile = true;
    [Tooltip("The input that must be pressed to reload the current scene.")]
    public KeyCode reloadScene = KeyCode.R;
    [Tooltip("In this variable it is possible to associate a 'UI Button' to execute the described command.")]
    public Button reloadSceneMobileButton;

    [Space(15)]
    [Tooltip("If this variable is true, the control for this variable will be activated.")]
    public bool enable_pause_Input_key = true;
    [Tooltip("If this variable is true, the control for this variable will be enabled on the mobile buttons.")]
    public bool enable_pause_Button_Mobile = true;
    [Tooltip("The input that must be pressed to pause the game.")]
    public KeyCode pause = KeyCode.P;
    [Tooltip("In this variable it is possible to associate a 'UI Button' to execute the described command.")]
    public Button pauseMobileButton;

    [Space(10)]
    [Header("Vehicle")]
    [Tooltip("If this variable is true, the control for this variable will be activated.")]
    public bool enable_startTheVehicle_Input_key = true;
    [Tooltip("If this variable is true, the control for this variable will be enabled on the mobile buttons.")]
    public bool enable_startTheVehicle_Button_Mobile = true;
    [Tooltip("The input that must be pressed to turn the vehicle engine on or off.")]
    public KeyCode startTheVehicle = KeyCode.F;
    [Tooltip("In this variable it is possible to associate a 'UI Button' to execute the described command.")]
    public Button startTheVehicleMobileButton;

    [Space(15)]
    [Tooltip("If this variable is true, the control for this variable will be activated.")]
    public bool enable_enterEndExit_Input_key = true;
    [Tooltip("If this variable is true, the control for this variable will be enabled on the mobile buttons.")]
    public bool enable_enterEndExit_Button_Mobile = true;
    [Tooltip("The input that must be pressed to enter or exit the vehicle.")]
    public KeyCode enterEndExit = KeyCode.T;
    [Tooltip("In this variable it is possible to associate a 'UI Button' to execute the described command.")]
    public Button enterEndExitMobileButton;

    [Space(15)]
    [Tooltip("If this variable is true, the control for this variable will be activated.")]
    public bool enable_hornInput_Input_key = true;
    [Tooltip("If this variable is true, the control for this variable will be enabled on the mobile buttons.")]
    public bool enable_hornInput_Button_Mobile = true;
    [Tooltip("The input that must be pressed to emit the horn sound of the vehicle.")]
    public KeyCode hornInput = KeyCode.B;
    [Tooltip("In this variable it is possible to associate a 'UI Button' to execute the described command.")]
    public Button hornMobileButton;

    [Space(15)]
    [Tooltip("If this variable is true, the control for this variable will be activated.")]
    public bool enable_handBrakeInput_Input_key = true;
    [Tooltip("If this variable is true, the control for this variable will be enabled on the mobile buttons.")]
    public bool enable_handBrakeInput_Button_Mobile = true;
    [Tooltip("The input that must be pressed to activate or deactivate the vehicle hand brake.")]
    public KeyCode handBrakeInput = KeyCode.Space;
    [Tooltip("In this variable it is possible to associate a 'UI Button' to execute the described command.")]
    public Button handBrakeMobileButton;

    [Space(15)]
    [Tooltip("If this variable is true, the control for this variable will be activated.")]
    public bool enable_switchingCameras_Input_key = true;
    [Tooltip("If this variable is true, the control for this variable will be enabled on the mobile buttons.")]
    public bool enable_switchingCameras_Button_Mobile = true;
    [Tooltip("The input that must be pressed to toggle between the cameras of the vehicle.")]
    public KeyCode switchingCameras = KeyCode.C;
    [Tooltip("In this variable it is possible to associate a 'UI Button' to execute the described command.")]
    public Button switchingCamerasMobileButton;

    [Space(15)]
    [Tooltip("If this variable is true, the control for this variable will be activated.")]
    public bool enable_changeSuspensionHeight_Input_key = true;
    [Tooltip("If this variable is true, the control for this variable will be enabled on the mobile buttons.")]
    public bool enable_changeSuspensionHeight_Button_Mobile = true;
    [Tooltip("The input that must be pressed to change the height of the vehicle.")]
    public KeyCode changeSuspensionHeight = KeyCode.I;
    [Tooltip("In this variable it is possible to associate a 'UI Button' to execute the described command.")]
    public Button changeSuspensionHeightMobileButton;

    [Space(15)]
    [Tooltip("If this variable is true, the control for this variable will be activated.")]
    public bool enable_nitro_Input_key = true;
    [Tooltip("If this variable is true, the control for this variable will be enabled on the mobile buttons.")]
    public bool enable_nitro_Button_Mobile = true;
    [Tooltip("The input that must be pressed to activate nitro mode of the vehicle.")]
    public KeyCode nitro = KeyCode.G;
    [Tooltip("In this variable it is possible to associate a 'UI Button' to execute the described command.")]
    public Button nitroMobileButton;
  }

  [Serializable]
  public class SensibilityControlsMobile {
    [Range(0.1f, 5.0f)]
    [Tooltip("Here you can set the speed at which you can zoom in or out of the vehicle camera.")]
    public float speedScrollWheelMobile = 0.5f;

    [Header("Mobile Joystick")]
    [Range(0.1f, 10.0f)]
    [Tooltip("The speed at which the input of the joystick will be passed to the vehicle.")]
    public float speedJoystickMove = 5.0f;
    [Range(0.1f, 10.0f)]
    [Tooltip("The speed at which the input of the joystick will be passed to the vehicle camera.")]
    public float speedJoystickCamera = 0.4f;

    [Space(10)]
    [Header("Mobile Button")]
    [Range(0.5f, 10.0f)]
    [Tooltip("The speed at which the input of the button will be passed to the vehicle.")]
    public float speedButtonMove = 5.0f;
    [Range(0.5f, 10.0f)]
    [Tooltip("The speed at which the input of the button will be passed to the vehicle.")]
    public float speedButtonDirection = 5.0f;
    [Range(0.1f, 10.0f)]
    [Tooltip("The speed at which the input of the joystick will be passed to the vehicle camera.")]
    public float _speedJoystickCamera = 0.4f;

    [Space(10)]
    [Header("Mobile Volant")]
    [Range(1.0f, 15.0f)]
    [Tooltip("The speed at which the joystick input will be passed to the steering wheel of the vehicle.")]
    public float speedMobileVolant = 10.0f;
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

    public enum ControlType { windows, mobileJoystick, mobileButton, mobileVolant };
    [Space(10)]
    [Tooltip("Here you can select the type of control, where 'Mobile Button' will cause buttons to appear on the screen so that vehicles can be controlled, 'Mobile Joystick' will cause two Joysticks to appear on the screen so vehicles can be controlled, 'windows' will allow vehicles to be controlled through the computer, and 'Mobile Volant' will make a steering wheel and two pedals appear on the screen, allowing you to control the vehicle through them.")]
    public ControlType selectControls = ControlType.windows;

    [Tooltip("Here you can configure the vehicle controls, choose the desired inputs and also, deactivate the unwanted ones.")]
    public Controls controls;
    [Tooltip("Here you can configure the sensitivity of the controls for mobile devices.")]
    public SensibilityControlsMobile sensitivityOfMobileControls;


    public enum StartMode { StartInThePlayer, StartInTheVehicle, AIInControl };
    [Space(10)]
    [Header("*SETTINGS")]
    [Tooltip("Here you can decide whether the game will start in the player ('Player' variable) or in any vehicle ('Starting Vehicle Name' variable). If there is no vehicle, the system will automatically start on the Player, and if there is no Player, the system will automatically start on a vehicle. If there is neither, nothing will start. If 'AI In Control' is selected, initial control will be passed to some AI.")]
    public StartMode _StartingMode = StartMode.StartInTheVehicle;
    [Tooltip("Here you can define the vehicle that starts in player control by the name of that vehicle.")]
    public string startingVehicleName = "VehicleName";
    [Tooltip("The player must be associated with this variable. This variable should only be used if your scene also has a player other than a vehicle. This 'player' will temporarily be disabled when you get in a vehicle, and will be activated again when you leave a vehicle.")]
    public GameObject player;
    [Tooltip("This is the minimum distance the player needs to be in relation to the door of a vehicle, to interact with it.")]
    public float minDistance = 3;

    [Space(10)]
    [Header("*UI TEXT")]
    [Tooltip("If this variable is true, a warning will appear on the screen every time the player approaches a vehicle, informing which key it is necessary to press to enter the vehicle.")]
    public bool pressKeyUI = true;

    [Space(10)]
    [Header("*OPTIMIZATION")]
    [Tooltip("If this variable is true, the vehicle controller will deactivate vehicles that are further away from the player in order to improve performance.")]
    public bool disableVehicleByDistance = true;
    [Tooltip("If the variable 'disableVehicleByDistance' is true, the code will disable vehicles that are far from the player, according to this variable. In this way, you can set the distance at which vehicles will be activated or deactivated.")]
    public float distanceDisable = 500;

    GameObject myCanvas;

    Joystick joystickMove;
    Joystick joystickRotate;
    Joystick joystickCamera;
    MSButton buttonLeft;
    MSButton buttonRight;
    MSButton buttonUp;
    MSButton buttonDown;
    MSButton buttonScrollUp;
    MSButton buttonScrollDown;
    Image backgroundMobile;

    Text enterOrExitVehicleText;

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

    [HideInInspector]
    public bool blockedInteraction = false;
    bool enterAndExitBoolMobile;
    bool enterAndExitTextBool;
    string sceneName;

    float MSbuttonHorizontal;
    float MSbuttonVertical;

    private MSVehicleController vehicleCode;
    [HideInInspector]
    public bool pause = false;

    bool interactBool;

    Vector2 vectorDirJoystick;

    //mobile volant variables
    Graphic androidInputVolant;
    RectTransform rectT;
    Vector2 centerPoint;
    private float maxAngle = 200.0f;
    private float rotateSpeed = 200.0f;
    float wheelAngle = 0.0f;
    float wheelPrevAngle = 0.0f;
    bool wheelBeingHeld = false;
    //end

    //ORDER = Awake > OnEnable > Start
    void Start() {
      vehicleCode = GetComponent<MSVehicleController>();
      vehicleCode.theEngineIsRunning = false;
      vehicleCode._vehicleState = MSVehicleController.ControlState.isNull;
      if (_StartingMode == StartMode.StartInThePlayer) {
        if (player) {
          player.SetActive(true);
          EnableOrDisableButtons(false);
        } else {
          _StartingMode = StartMode.StartInTheVehicle;
          vehicleCode.EnterInVehicle(true); //vehicle state >> isPlayer
          EnableOrDisableButtons(true);
          vehicleCode.theEngineIsRunning = vehicleCode._vehicleSettings.startOn;
        }
      }
      if (_StartingMode == StartMode.StartInTheVehicle) {
        if (player) {
          player.SetActive(false);
        }
        vehicleCode.EnterInVehicle(true); //vehicle state >> isPlayer
        EnableOrDisableButtons(true);
        vehicleCode.theEngineIsRunning = vehicleCode._vehicleSettings.startOn;
      }
      if (_StartingMode == StartMode.AIInControl) {
        vehicleCode.EnterInVehicle(false); //vehicle state >> isAI
        EnableOrDisableButtons(false);
        vehicleCode.theEngineIsRunning = vehicleCode._vehicleSettings.startOn;
      }
    }

    #region UPDATE REGION
    void SetUpFirstVehicleOnRunTime() {
      if (_StartingMode == StartMode.StartInTheVehicle) {
        if (player) {
          if (!player.activeInHierarchy) {
            vehicleCode.EnterInVehicle(true); //vehicle state >> isPlayer
            EnableOrDisableButtons(true);
            vehicleCode.theEngineIsRunning = vehicleCode._vehicleSettings.startOn;
          }
        } else {
          vehicleCode.EnterInVehicle(true); //vehicle state >> isPlayer
          EnableOrDisableButtons(true);
          vehicleCode.theEngineIsRunning = vehicleCode._vehicleSettings.startOn;
        }
      }
      if (_StartingMode == StartMode.AIInControl) {
        vehicleCode.EnterInVehicle(false); //vehicle state >> isAI
        EnableOrDisableButtons(false);
        vehicleCode.theEngineIsRunning = vehicleCode._vehicleSettings.startOn;
      }
      if (_StartingMode == StartMode.StartInThePlayer) {
        vehicleCode.ExitTheVehicle();
        EnableOrDisableButtons(false);
      }
    }

    void Update() {

      #region customizeMainInputsValues
      switch (selectControls) {
        case ControlType.mobileVolant:
          if (androidInputVolant) {
            if (!wheelBeingHeld && !Mathf.Approximately(0.0f, wheelAngle)) {
              float deltaAngle = rotateSpeed * Time.deltaTime;
              if (Mathf.Abs(deltaAngle) > Mathf.Abs(wheelAngle)) {
                wheelAngle = 0.0f;
              } else if (wheelAngle > 0.0f) {
                wheelAngle -= deltaAngle;
              } else {
                wheelAngle += deltaAngle;
              }
            }
            MSbuttonHorizontal = (wheelAngle / maxAngle);
            rectT.localEulerAngles = Vector3.back * wheelAngle;
          }
          if (buttonUp && buttonDown) {
            MSbuttonVertical = (-buttonDown.buttonInput + buttonUp.buttonInput);
          }
          if (joystickCamera) {
            mouseXInput = Mathf.Lerp(mouseXInput, joystickCamera.joystickHorizontal * sensitivityOfMobileControls._speedJoystickCamera, Time.deltaTime * 5);
            mouseYInput = Mathf.Lerp(mouseYInput, joystickCamera.joystickVertical * sensitivityOfMobileControls._speedJoystickCamera, Time.deltaTime * 5);
          }
          verticalInput = Mathf.Lerp(verticalInput, MSbuttonVertical, Time.deltaTime * sensitivityOfMobileControls.speedButtonMove);
          horizontalInput = Mathf.Lerp(horizontalInput, MSbuttonHorizontal, Time.deltaTime * sensitivityOfMobileControls.speedMobileVolant);
          //
          if (buttonScrollUp && buttonScrollDown) {
            mouseScrollWheelInput = (-buttonScrollDown.buttonInput + buttonScrollUp.buttonInput) * Time.deltaTime * sensitivityOfMobileControls.speedScrollWheelMobile;
          }
          break;
        case ControlType.mobileButton:
          if (buttonLeft && buttonRight) {
            MSbuttonHorizontal = (-buttonLeft.buttonInput + buttonRight.buttonInput);
          }
          if (buttonUp && buttonDown) {
            MSbuttonVertical = (-buttonDown.buttonInput + buttonUp.buttonInput);
          }
          if (joystickCamera) {
            mouseXInput = Mathf.Lerp(mouseXInput, joystickCamera.joystickHorizontal * sensitivityOfMobileControls._speedJoystickCamera, Time.deltaTime * 5);
            mouseYInput = Mathf.Lerp(mouseYInput, joystickCamera.joystickVertical * sensitivityOfMobileControls._speedJoystickCamera, Time.deltaTime * 5);
          }
          verticalInput = Mathf.Lerp(verticalInput, MSbuttonVertical, Time.deltaTime * sensitivityOfMobileControls.speedButtonMove);
          horizontalInput = Mathf.Lerp(horizontalInput, MSbuttonHorizontal, Time.deltaTime * sensitivityOfMobileControls.speedButtonDirection);
          //
          if (buttonScrollUp && buttonScrollDown) {
            mouseScrollWheelInput = (-buttonScrollDown.buttonInput + buttonScrollUp.buttonInput) * Time.deltaTime * sensitivityOfMobileControls.speedScrollWheelMobile;
          }
          break;
        //====================================================================================
        case ControlType.mobileJoystick:
          if (joystickMove || joystickRotate) {
            if (joystickMove) {
              vectorDirJoystick = new Vector2(joystickMove.joystickHorizontal, joystickMove.joystickVertical);
              if (vectorDirJoystick.magnitude > 1) {
                vectorDirJoystick.Normalize();
              }
              if (joystickMove.joystickVertical >= 0) {
                verticalInput = vectorDirJoystick.magnitude;
              } else {
                verticalInput = -vectorDirJoystick.magnitude;
              }
              horizontalInput = Mathf.Lerp(horizontalInput, joystickMove.joystickHorizontal, Time.deltaTime * sensitivityOfMobileControls.speedJoystickMove);
            }
            if (joystickRotate) {
              mouseXInput = Mathf.Lerp(mouseXInput, joystickRotate.joystickHorizontal * sensitivityOfMobileControls.speedJoystickCamera, Time.deltaTime * 5.0f);
              mouseYInput = Mathf.Lerp(mouseYInput, joystickRotate.joystickVertical * sensitivityOfMobileControls.speedJoystickCamera, Time.deltaTime * 5.0f);
            }
            //
            if (buttonScrollUp && buttonScrollDown) {
              mouseScrollWheelInput = (-buttonScrollDown.buttonInput + buttonScrollUp.buttonInput) * Time.deltaTime * sensitivityOfMobileControls.speedScrollWheelMobile;
            }
          }
          break;
        //====================================================================================
        case ControlType.windows:
          verticalInput = Mathf.Clamp(Input.GetAxis(_verticalInput), -1, 1);
          horizontalInput = Mathf.Clamp(Input.GetAxis(_horizontalInput), -1, 1);
          mouseXInput = Input.GetAxis(_mouseXInput);
          mouseYInput = Input.GetAxis(_mouseYInput);
          mouseScrollWheelInput = Input.GetAxis(_mouseScrollWheelInput);
          break;
          //====================================================================================
      }
      #endregion

      //pause input
      if (controls.enable_pause_Input_key) {
        if (Input.GetKeyDown(controls.pause)) {
          pause = !pause;
        }
        if (pause) {
          Time.timeScale = Mathf.Lerp(Time.timeScale, 0.0f, Time.fixedDeltaTime * 5.0f);
        } else {
          Time.timeScale = Mathf.Lerp(Time.timeScale, 1.0f, Time.fixedDeltaTime * 5.0f);
        }
      }

      bool _insideTheCar = false;
      if (vehicleCode._vehicleState == MSVehicleController.ControlState.isPlayer) {
        _insideTheCar = true;
      }
      EnableOrDisableButtons(_insideTheCar);

      //enter end exit 
      if ((Input.GetKeyDown(controls.enterEndExit) || enterAndExitBoolMobile) && !blockedInteraction && player && controls.enable_enterEndExit_Input_key) {
        if (vehicleCode.transform.gameObject.activeInHierarchy) {
          if (_insideTheCar) {
            vehicleCode.ExitTheVehicle();
            if (player) {
              int freeDoor = 0;
              for (int x = 0; x < vehicleCode.doorPosition.Length; x++) {
                bool checkObstacles = CheckObstacles(vehicleCode.doorPosition[x].transform);
                if (checkObstacles) {
                  freeDoor++;
                } else {
                  break;
                }
              }
              //
              if (freeDoor < vehicleCode.doorPosition.Length) {
                player.transform.position = vehicleCode.doorPosition[freeDoor].transform.position;
              } else {
                player.transform.position = vehicleCode.doorPosition[0].transform.position + Vector3.up * 3.0f;
              }
              player.SetActive(true);
            }
            blockedInteraction = true;
            enterAndExitBoolMobile = false;
            StartCoroutine("WaitToInteract");
          } else {
            float currentDistance = Vector3.Distance(player.transform.position, vehicleCode.doorPosition[0].transform.position);
            for (int x = 0; x < vehicleCode.doorPosition.Length; x++) {
              float proximityDistance = Vector3.Distance(player.transform.position, vehicleCode.doorPosition[x].transform.position);
              if (proximityDistance < currentDistance) {
                currentDistance = proximityDistance;
              }
            }
            if (currentDistance < minDistance) {
              vehicleCode.EnterInVehicle(true); //true = isplayer
              if (player) {
                player.SetActive(false);
              }
              blockedInteraction = true;
              enterAndExitBoolMobile = false;
              StartCoroutine("WaitToInteract");
            } else {
              enterAndExitBoolMobile = false;
            }
          }
        }
      }


      //enable or disable text (Press X to enter in vehicle)
      if (player) {
        if (!enterAndExitTextBool) {
          enterAndExitTextBool = true;
          StartCoroutine(WaitToCheckDistance(_insideTheCar));
        }
      }


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
          //
          //gears
          if (selectControls == MSSceneController.ControlType.windows) {
            //input manual or automatic gears
            if (Input.GetKeyDown(controls.manualOrAutoGears) && controls.enable_manualOrAutoGears_Input_key) {
              vehicleCode.automaticGears = !vehicleCode.automaticGears;
            }

            //gears logic
            if (vehicleCode.automaticGears) {
              if (Input.GetKey(controls.handBrakeInput) && controls.enable_handBrakeInput_Input_key) {
                vehicleCode.handBrakeTrue = true;
              } else {
                vehicleCode.handBrakeTrue = false;
              }
            } else {
              if (Input.GetKeyDown(controls.handBrakeInput) && controls.enable_handBrakeInput_Input_key) {
                vehicleCode.handBrakeTrue = !vehicleCode.handBrakeTrue;
              }
            }

            //up and down gear
            if (!vehicleCode.automaticGears) {
              if (Input.GetKeyDown(controls.increasedGear) && controls.enable_increasedGear_Input_key && vehicleCode.currentGear < vehicleCode._vehicleTorque.numberOfGears && !vehicleCode.changinGears) {
                vehicleCode.StartCoroutine("ChangeGears", vehicleCode.currentGear + 1);
              }
              if (Input.GetKeyDown(controls.decreasedGear) && controls.enable_decreasedGear_Input_key && vehicleCode.currentGear > -1 && !vehicleCode.changinGears) {
                vehicleCode.StartCoroutine("ChangeGears", vehicleCode.currentGear - 1);
              }
            }

            //horn input
            if (Input.GetKeyDown(controls.hornInput) && controls.enable_hornInput_Input_key) {
              vehicleCode.hornIsOn = true;
            }

            //turn on and turn off input
            if (vehicleCode.youCanCall && controls.enable_startTheVehicle_Input_key) {
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

            //change suspension height input
            if (controls.enable_changeSuspensionHeight_Input_key) {
              if (vehicleCode._suspension.vehicleCustomHeights.Length > 0) {
                if (Input.GetKeyDown(controls.changeSuspensionHeight)) {
                  if (vehicleCode._suspension.indexCustomSuspensionHeight < (vehicleCode._suspension.vehicleCustomHeights.Length - 1)) {
                    vehicleCode._suspension.indexCustomSuspensionHeight++;
                  } else if (vehicleCode._suspension.indexCustomSuspensionHeight >= (vehicleCode._suspension.vehicleCustomHeights.Length - 1)) {
                    vehicleCode._suspension.indexCustomSuspensionHeight = 0;
                  }
                }
              }
            }

            //NITRO input
            if (controls.enable_nitro_Input_key) {
              if (vehicleCode._additionalFeatures.useNitro) {
                if (Input.GetKeyDown(controls.nitro)) {
                  vehicleCode._additionalFeatures.nitroIsTrueInput = !vehicleCode._additionalFeatures.nitroIsTrueInput;
                }
              }
            }

            //LIGHTS
            //main lights
            if (Input.GetKeyDown(controls.mainLightsInput) && controls.enable_mainLightsInput_Input_key) {
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
            if (Input.GetKeyDown(controls.headlightsInput) && controls.enable_headlightsInput_Input_key) {
              vehicleCode.headlightsOn = !vehicleCode.headlightsOn;
            }
            //flashesRightAlert
            if (controls.enable_flashesRightAlert_Input_key) {
              if (Input.GetKeyDown(controls.flashesRightAlert) && !vehicleCode.rightBlinkersOn && !vehicleCode.alertOn) {
                vehicleCode.rightBlinkersOn = true;
                vehicleCode.leftBlinkersOn = false;
                vehicleCode.disableBlinkers1 = true;
              } else if (Input.GetKeyDown(controls.flashesRightAlert) && vehicleCode.rightBlinkersOn && !vehicleCode.alertOn) {
                vehicleCode.rightBlinkersOn = false;
                vehicleCode.leftBlinkersOn = false;
                vehicleCode.disableBlinkers1 = false;
              }
            }
            //flashesLeftAlert
            if (controls.enable_flashesLeftAlert_Input_key) {
              if (Input.GetKeyDown(controls.flashesLeftAlert) && !vehicleCode.leftBlinkersOn && !vehicleCode.alertOn) {
                vehicleCode.rightBlinkersOn = false;
                vehicleCode.leftBlinkersOn = true;
                vehicleCode.disableBlinkers1 = true;
              } else if (Input.GetKeyDown(controls.flashesLeftAlert) && vehicleCode.leftBlinkersOn && !vehicleCode.alertOn) {
                vehicleCode.rightBlinkersOn = false;
                vehicleCode.leftBlinkersOn = false;
                vehicleCode.disableBlinkers1 = false;
              }
            }
            //alertOn
            if (Input.GetKeyDown(controls.warningLightsInput) && controls.enable_warningLightsInput_Input_key) {
              if (vehicleCode.alertOn) {
                vehicleCode.alertOn = false;
                vehicleCode.rightBlinkersOn = vehicleCode.leftBlinkersOn = false;
              } else {
                vehicleCode.alertOn = true;
                vehicleCode.rightBlinkersOn = vehicleCode.leftBlinkersOn = true;
              }
            }
            //extraLightsOn
            if (Input.GetKeyDown(controls.extraLightsInput) && controls.enable_extraLightsInput_Input_key) {
              vehicleCode.extraLightsOn = !vehicleCode.extraLightsOn;
            }
          } else {
            //mobile inputs
            if (vehicleCode.automaticGears) {
              vehicleCode.handBrakeTrue = false;
            }
          }
        }
      }
    }

    bool CheckObstacles(Transform vehicleDoor) {
      Collider[] hitColliders = Physics.OverlapSphere(vehicleDoor.position, 0.4f);
      if (hitColliders.Length > 0) {
        return true;
      }
      return false;
    }

    IEnumerator WaitToInteract() {
      yield return new WaitForSeconds(0.7f);
      blockedInteraction = false;
    }

    IEnumerator WaitToCheckDistance(bool isInsideTheCar) {
      interactBool = false;
      if (!isInsideTheCar) {
        if (vehicleCode.transform.gameObject.activeInHierarchy && vehicleCode.enabled) {
          for (int y = 0; y < vehicleCode.doorPosition.Length; y++) {
            if (Vector3.Distance(player.transform.position, vehicleCode.doorPosition[y].transform.position) < minDistance) {
              interactBool = true;
            }
          }
        }
        if (interactBool && pressKeyUI) {
          if (selectControls == ControlType.mobileButton || selectControls == ControlType.mobileJoystick || selectControls == ControlType.mobileVolant) {
            enterOrExitVehicleText.enabled = false;
          } else {
            enterOrExitVehicleText.enabled = true;
          }
        } else {
          enterOrExitVehicleText.enabled = false;
        }
      } else {
        enterOrExitVehicleText.enabled = false;
      }
      //
      yield return new WaitForSeconds(0.2f);
      enterAndExitTextBool = false;
    }
    #endregion



    #region MOBILE REGION
    void SetVoidsOnMobileButtons() {
      //gears
      if (controls.manualOrAutoGearsMobileButton) {
        controls.manualOrAutoGearsMobileButton.onClick = new Button.ButtonClickedEvent();
        controls.manualOrAutoGearsMobileButton.onClick.AddListener(() => Mobile_ManualOrAutoGearsButton());
      } else {
        controls.enable_manualOrAutoGears_Button_Mobile = false;
      }

      if (controls.increasedGearMobileButton) {
        controls.increasedGearMobileButton.onClick = new Button.ButtonClickedEvent();
        controls.increasedGearMobileButton.onClick.AddListener(() => Mobile_IncreasedGearButton());
      } else {
        controls.enable_increasedGear_Button_Mobile = false;
      }

      if (controls.decreasedGearMobileButton) {
        controls.decreasedGearMobileButton.onClick = new Button.ButtonClickedEvent();
        controls.decreasedGearMobileButton.onClick.AddListener(() => Mobile_DecreasedGearButton());
      } else {
        controls.enable_decreasedGear_Button_Mobile = false;
      }

      //
      //
      //lights
      if (controls.flashesRightAlertMobileButton) {
        controls.flashesRightAlertMobileButton.onClick = new Button.ButtonClickedEvent();
        controls.flashesRightAlertMobileButton.onClick.AddListener(() => Mobile_FlashesRightAlertButton());
      } else {
        controls.enable_flashesRightAlert_Button_Mobile = false;
      }

      if (controls.flashesLeftAlertMobileButton) {
        controls.flashesLeftAlertMobileButton.onClick = new Button.ButtonClickedEvent();
        controls.flashesLeftAlertMobileButton.onClick.AddListener(() => Mobile_FlashesLeftAlertButton());
      } else {
        controls.enable_flashesLeftAlert_Button_Mobile = false;
      }

      if (controls.mainLightsMobileButton) {
        controls.mainLightsMobileButton.onClick = new Button.ButtonClickedEvent();
        controls.mainLightsMobileButton.onClick.AddListener(() => Mobile_MainLightsButton());
      } else {
        controls.enable_mainLightsInput_Button_Mobile = false;
      }

      if (controls.extraLightsMobileButton) {
        controls.extraLightsMobileButton.onClick = new Button.ButtonClickedEvent();
        controls.extraLightsMobileButton.onClick.AddListener(() => Mobile_ExtraLightsButton());
      } else {
        controls.enable_extraLightsInput_Button_Mobile = false;
      }

      if (controls.headlightsMobileButton) {
        controls.headlightsMobileButton.onClick = new Button.ButtonClickedEvent();
        controls.headlightsMobileButton.onClick.AddListener(() => Mobile_HeadLightsButton());
      } else {
        controls.enable_headlightsInput_Button_Mobile = false;
      }

      if (controls.warningLightsMobileButton) {
        controls.warningLightsMobileButton.onClick = new Button.ButtonClickedEvent();
        controls.warningLightsMobileButton.onClick.AddListener(() => Mobile_WarningLightsButton());
      } else {
        controls.enable_warningLightsInput_Button_Mobile = false;
      }

      //
      //
      //game
      if (controls.reloadSceneMobileButton) {
        controls.reloadSceneMobileButton.onClick = new Button.ButtonClickedEvent();
        controls.reloadSceneMobileButton.onClick.AddListener(() => Mobile_ReloadSceneButton());
      } else {
        controls.enable_reloadScene_Button_Mobile = false;
      }

      if (controls.pauseMobileButton) {
        controls.pauseMobileButton.onClick = new Button.ButtonClickedEvent();
        controls.pauseMobileButton.onClick.AddListener(() => Mobile_PauseButton());
      } else {
        controls.enable_pause_Button_Mobile = false;
      }

      //
      //
      //vehicle
      if (controls.startTheVehicleMobileButton) {
        controls.startTheVehicleMobileButton.onClick = new Button.ButtonClickedEvent();
        controls.startTheVehicleMobileButton.onClick.AddListener(() => Mobile_StartTheVehicleButton());
      } else {
        controls.enable_startTheVehicle_Button_Mobile = false;
      }

      if (controls.enterEndExitMobileButton) {
        controls.enterEndExitMobileButton.onClick = new Button.ButtonClickedEvent();
        controls.enterEndExitMobileButton.onClick.AddListener(() => Mobile_EnterAndExitButton());
      } else {
        controls.enable_enterEndExit_Button_Mobile = false;
      }

      if (controls.hornMobileButton) {
        controls.hornMobileButton.onClick = new Button.ButtonClickedEvent();
        controls.hornMobileButton.onClick.AddListener(() => Mobile_HornButton());
      } else {
        controls.enable_hornInput_Button_Mobile = false;
      }

      if (controls.handBrakeMobileButton) {
        controls.handBrakeMobileButton.onClick = new Button.ButtonClickedEvent();
        controls.handBrakeMobileButton.onClick.AddListener(() => Mobile_HandBrakeButton());
      } else {
        controls.enable_handBrakeInput_Button_Mobile = false;
      }

      if (controls.changeSuspensionHeightMobileButton) {
        controls.changeSuspensionHeightMobileButton.onClick = new Button.ButtonClickedEvent();
        controls.changeSuspensionHeightMobileButton.onClick.AddListener(() => Mobile_ChangeSuspensionHeightInputButton());
      } else {
        controls.enable_changeSuspensionHeight_Button_Mobile = false;
      }

      if (controls.nitroMobileButton) {
        controls.nitroMobileButton.onClick = new Button.ButtonClickedEvent();
        controls.nitroMobileButton.onClick.AddListener(() => Mobile_NitroMobileButtonInputButton());
      } else {
        controls.enable_nitro_Button_Mobile = false;
      }
    }
    void EnableOrDisableButtons(bool insideInCar) { //on Update
      if (selectControls == ControlType.mobileButton || selectControls == ControlType.mobileJoystick || selectControls == ControlType.mobileVolant) {
        if (backgroundMobile) {
          if (insideInCar) {
            backgroundMobile.enabled = true;
          } else {
            backgroundMobile.enabled = false;
          }
        }
        //gears
        if (controls.manualOrAutoGearsMobileButton) {
          controls.manualOrAutoGearsMobileButton.gameObject.SetActive(insideInCar & controls.enable_manualOrAutoGears_Button_Mobile);
        }
        if (vehicleCode.automaticGears) {
          if (controls.increasedGearMobileButton) {
            controls.increasedGearMobileButton.gameObject.SetActive(false);
          }
          if (controls.decreasedGearMobileButton) {
            controls.decreasedGearMobileButton.gameObject.SetActive(false);
          }
        } else {
          if (controls.increasedGearMobileButton) {
            controls.increasedGearMobileButton.gameObject.SetActive(insideInCar & controls.enable_increasedGear_Button_Mobile);
          }
          if (controls.decreasedGearMobileButton) {
            controls.decreasedGearMobileButton.gameObject.SetActive(insideInCar & controls.enable_decreasedGear_Button_Mobile);
          }
        }


        //lights
        if (vehicleCode.msvs_useWarningOrFlashingLights) {
          if (controls.flashesRightAlertMobileButton) {
            controls.flashesRightAlertMobileButton.gameObject.SetActive(insideInCar & controls.enable_flashesRightAlert_Button_Mobile);
          }
          if (controls.flashesLeftAlertMobileButton) {
            controls.flashesLeftAlertMobileButton.gameObject.SetActive(insideInCar & controls.enable_flashesLeftAlert_Button_Mobile);
          }
          if (controls.warningLightsMobileButton) {
            controls.warningLightsMobileButton.gameObject.SetActive(insideInCar & controls.enable_warningLightsInput_Button_Mobile);
          }
        } else {
          if (controls.flashesRightAlertMobileButton) {
            controls.flashesRightAlertMobileButton.gameObject.SetActive(false);
          }
          if (controls.flashesLeftAlertMobileButton) {
            controls.flashesLeftAlertMobileButton.gameObject.SetActive(false);
          }
          if (controls.warningLightsMobileButton) {
            controls.warningLightsMobileButton.gameObject.SetActive(false);
          }
        }
        //
        if (controls.mainLightsMobileButton) {
          if (vehicleCode.msvs_useMainLights) {
            controls.mainLightsMobileButton.gameObject.SetActive(insideInCar & controls.enable_mainLightsInput_Button_Mobile);
          } else {
            controls.mainLightsMobileButton.gameObject.SetActive(false);
          }
        }
        //
        if (controls.headlightsMobileButton) {
          if (vehicleCode.msvs_useHeadLights) {
            controls.headlightsMobileButton.gameObject.SetActive(insideInCar & controls.enable_headlightsInput_Button_Mobile);
          } else {
            controls.headlightsMobileButton.gameObject.SetActive(false);
          }
        }
        //
        if (controls.extraLightsMobileButton) {
          if (vehicleCode.msvs_useExtraLights) {
            controls.extraLightsMobileButton.gameObject.SetActive(insideInCar & controls.enable_extraLightsInput_Button_Mobile);
          } else {
            controls.extraLightsMobileButton.gameObject.SetActive(false);
          }
        }

        //game
        if (controls.reloadSceneMobileButton) {
          controls.reloadSceneMobileButton.gameObject.SetActive(controls.enable_reloadScene_Button_Mobile);
        }
        if (controls.pauseMobileButton) {
          controls.pauseMobileButton.gameObject.SetActive(controls.enable_pause_Button_Mobile);
        }

        //vehicle
        if (controls.startTheVehicleMobileButton) {
          controls.startTheVehicleMobileButton.gameObject.SetActive(insideInCar & controls.enable_startTheVehicle_Button_Mobile);
        }
        if (controls.enterEndExitMobileButton) {
          if (player) {
            controls.enterEndExitMobileButton.gameObject.SetActive(controls.enable_enterEndExit_Button_Mobile);
          } else {
            controls.enterEndExitMobileButton.gameObject.SetActive(false);
          }
        }
        if (controls.hornMobileButton) {
          if (vehicleCode._sounds.hornSound) {
            controls.hornMobileButton.gameObject.SetActive(insideInCar & controls.enable_hornInput_Button_Mobile);
          } else {
            controls.hornMobileButton.gameObject.SetActive(false);
          }
        }
        if (controls.handBrakeMobileButton) {
          if (vehicleCode.automaticGears) {
            controls.handBrakeMobileButton.gameObject.SetActive(false);
          } else {
            controls.handBrakeMobileButton.gameObject.SetActive(insideInCar & controls.enable_handBrakeInput_Button_Mobile);
          }
        }
        if (controls.changeSuspensionHeightMobileButton) {
          if (vehicleCode._suspension.vehicleCustomHeights.Length <= 0) {
            controls.changeSuspensionHeightMobileButton.gameObject.SetActive(false);
          } else {
            controls.changeSuspensionHeightMobileButton.gameObject.SetActive(insideInCar & controls.enable_changeSuspensionHeight_Button_Mobile);
          }
        }
        if (controls.nitroMobileButton) {
          if (vehicleCode._additionalFeatures.useNitro) {
            controls.nitroMobileButton.gameObject.SetActive(insideInCar & controls.enable_nitro_Button_Mobile);
          } else {
            controls.nitroMobileButton.gameObject.SetActive(false);
          }
        }


        /// JOYSTICK, BUTTON AND MOVE CONTROLLS
        if (selectControls == ControlType.mobileButton) {
          //joystick
          if (joystickMove) {
            joystickMove.gameObject.SetActive(false);
          }
          if (joystickRotate) {
            joystickRotate.gameObject.SetActive(false);
          }

          //move buttons
          if (buttonLeft) {
            buttonLeft.gameObject.SetActive(insideInCar);
          }
          if (buttonRight) {
            buttonRight.gameObject.SetActive(insideInCar);
          }
          if (buttonUp) {
            buttonUp.gameObject.SetActive(insideInCar);
          }
          if (buttonDown) {
            buttonDown.gameObject.SetActive(insideInCar);
          }
          //volant
          if (androidInputVolant) {
            androidInputVolant.gameObject.SetActive(false);
          }
        }
        // \/
        if (selectControls == ControlType.mobileJoystick) {
          //joystick
          if (joystickMove) {
            joystickMove.gameObject.SetActive(insideInCar);
          }

          //move buttons
          if (buttonLeft) {
            buttonLeft.gameObject.SetActive(false);
          }
          if (buttonRight) {
            buttonRight.gameObject.SetActive(false);
          }
          if (buttonUp) {
            buttonUp.gameObject.SetActive(false);
          }
          if (buttonDown) {
            buttonDown.gameObject.SetActive(false);
          }
          //volant
          if (androidInputVolant) {
            androidInputVolant.gameObject.SetActive(false);
          }
        }
        // \/
        if (selectControls == ControlType.mobileVolant) {
          //joystick
          if (joystickMove) {
            joystickMove.gameObject.SetActive(false);
          }
          if (joystickRotate) {
            joystickRotate.gameObject.SetActive(false);
          }

          //move buttons
          if (buttonLeft) {
            buttonLeft.gameObject.SetActive(false);
          }
          if (buttonRight) {
            buttonRight.gameObject.SetActive(false);
          }
          if (buttonUp) {
            buttonUp.gameObject.SetActive(insideInCar);
          }
          if (buttonDown) {
            buttonDown.gameObject.SetActive(insideInCar);
          }
          //volant
          if (androidInputVolant) {
            androidInputVolant.gameObject.SetActive(insideInCar);
          }
        }
      } else {
        if (backgroundMobile) {
          backgroundMobile.enabled = false;
        }
        //gears
        if (controls.manualOrAutoGearsMobileButton) {
          controls.manualOrAutoGearsMobileButton.gameObject.SetActive(false);
        }
        if (controls.increasedGearMobileButton) {
          controls.increasedGearMobileButton.gameObject.SetActive(false);
        }
        if (controls.decreasedGearMobileButton) {
          controls.decreasedGearMobileButton.gameObject.SetActive(false);
        }

        //lights
        if (controls.flashesRightAlertMobileButton) {
          controls.flashesRightAlertMobileButton.gameObject.SetActive(false);
        }
        if (controls.flashesLeftAlertMobileButton) {
          controls.flashesLeftAlertMobileButton.gameObject.SetActive(false);
        }
        if (controls.mainLightsMobileButton) {
          controls.mainLightsMobileButton.gameObject.SetActive(false);
        }
        if (controls.extraLightsMobileButton) {
          controls.extraLightsMobileButton.gameObject.SetActive(false);
        }
        if (controls.headlightsMobileButton) {
          controls.headlightsMobileButton.gameObject.SetActive(false);
        }
        if (controls.warningLightsMobileButton) {
          controls.warningLightsMobileButton.gameObject.SetActive(false);
        }

        //game
        if (controls.reloadSceneMobileButton) {
          controls.reloadSceneMobileButton.gameObject.SetActive(false);
        }
        if (controls.pauseMobileButton) {
          controls.pauseMobileButton.gameObject.SetActive(false);
        }

        //vehicle
        if (controls.startTheVehicleMobileButton) {
          controls.startTheVehicleMobileButton.gameObject.SetActive(false);
        }
        if (controls.enterEndExitMobileButton) {
          controls.enterEndExitMobileButton.gameObject.SetActive(false);
        }
        if (controls.hornMobileButton) {
          controls.hornMobileButton.gameObject.SetActive(false);
        }
        if (controls.handBrakeMobileButton) {
          controls.handBrakeMobileButton.gameObject.SetActive(false);
        }
        if (controls.switchingCamerasMobileButton) {
          controls.switchingCamerasMobileButton.gameObject.SetActive(false);
        }
        if (controls.changeSuspensionHeightMobileButton) {
          controls.changeSuspensionHeightMobileButton.gameObject.SetActive(false);
        }
        if (controls.nitroMobileButton) {
          controls.nitroMobileButton.gameObject.SetActive(false);
        }

        //
        if (joystickMove) {
          joystickMove.gameObject.SetActive(false);
        }
        if (joystickRotate) {
          joystickRotate.gameObject.SetActive(false);
        }
        if (joystickCamera) {
          joystickCamera.gameObject.SetActive(false);
        }
        if (buttonScrollUp) {
          buttonScrollUp.gameObject.SetActive(false);
        }
        if (buttonScrollDown) {
          buttonScrollDown.gameObject.SetActive(false);
        }
        if (buttonLeft) {
          buttonLeft.gameObject.SetActive(false);
        }
        if (buttonRight) {
          buttonRight.gameObject.SetActive(false);
        }
        if (buttonUp) {
          buttonUp.gameObject.SetActive(false);
        }
        if (buttonDown) {
          buttonDown.gameObject.SetActive(false);
        }
        if (androidInputVolant) {
          androidInputVolant.gameObject.SetActive(false);
        }
      }
    }

    #region mobileVolant
    public float GetClampedValue() {
      return wheelAngle / maxAngle;
    }
    public float GetAngle() {
      return wheelAngle;
    }
    void InitEventsSystem() {
      EventTrigger events = androidInputVolant.gameObject.GetComponent<EventTrigger>();
      if (events == null) {
        events = androidInputVolant.gameObject.AddComponent<EventTrigger>();
      }
      if (events.triggers == null) {
        events.triggers = new System.Collections.Generic.List<EventTrigger.Entry>();
      }
      EventTrigger.Entry entry = new EventTrigger.Entry();
      EventTrigger.TriggerEvent callback = new EventTrigger.TriggerEvent();
      UnityAction<BaseEventData> functionCall = new UnityAction<BaseEventData>(PressEvent);
      callback.AddListener(functionCall);
      entry.eventID = EventTriggerType.PointerDown;
      entry.callback = callback;
      events.triggers.Add(entry);
      entry = new EventTrigger.Entry();
      callback = new EventTrigger.TriggerEvent();
      functionCall = new UnityAction<BaseEventData>(DragEvent);
      callback.AddListener(functionCall);
      entry.eventID = EventTriggerType.Drag;
      entry.callback = callback;
      events.triggers.Add(entry);
      entry = new EventTrigger.Entry();
      callback = new EventTrigger.TriggerEvent();
      functionCall = new UnityAction<BaseEventData>(ReleaseEvent);
      callback.AddListener(functionCall);
      entry.eventID = EventTriggerType.PointerUp;
      entry.callback = callback;
      events.triggers.Add(entry);
    }
    void UpdateRect() {
      Vector3[] corners = new Vector3[4];
      rectT.GetWorldCorners(corners);
      for (int i = 0; i < 4; i++) {
        corners[i] = RectTransformUtility.WorldToScreenPoint(null, corners[i]);
      }
      Vector3 bottomLeft = corners[0];
      Vector3 topRight = corners[2];
      float width = topRight.x - bottomLeft.x;
      float height = topRight.y - bottomLeft.y;
      Rect _rect = new Rect(bottomLeft.x, topRight.y, width, height);
      centerPoint = new Vector2(_rect.x + _rect.width * 0.5f, _rect.y - _rect.height * 0.5f);
    }
    public void PressEvent(BaseEventData eventData) {
      Vector2 pointerPos = ((PointerEventData)eventData).position;
      wheelBeingHeld = true;
      wheelPrevAngle = Vector2.Angle(Vector2.up, pointerPos - centerPoint);
    }
    public void DragEvent(BaseEventData eventData) {
      Vector2 pointerPos = ((PointerEventData)eventData).position;
      float wheelNewAngle = Vector2.Angle(Vector2.up, pointerPos - centerPoint);
      if (Vector2.Distance(pointerPos, centerPoint) > 20f) {
        if (pointerPos.x > centerPoint.x) {
          wheelAngle += wheelNewAngle - wheelPrevAngle;
        } else {
          wheelAngle -= wheelNewAngle - wheelPrevAngle;
        }
      }
      wheelAngle = Mathf.Clamp(wheelAngle, -maxAngle, maxAngle);
      wheelPrevAngle = wheelNewAngle;
    }
    public void ReleaseEvent(BaseEventData eventData) {
      DragEvent(eventData);
      wheelBeingHeld = false;
    }
    #endregion

    #region mobileButtonInputs
    //gears
    void Mobile_IncreasedGearButton() {
      if (controls.enable_increasedGear_Button_Mobile) {
        if (vehicleCode) {
          if (vehicleCode._vehicleState == MSVehicleController.ControlState.isPlayer) {
            if (vehicleCode.currentGear < vehicleCode._vehicleTorque.numberOfGears && !vehicleCode.changinGears) {
              vehicleCode.StartCoroutine("ChangeGears", vehicleCode.currentGear + 1);
            }
          }
        }
      }
    }
    void Mobile_DecreasedGearButton() {
      if (controls.enable_decreasedGear_Button_Mobile) {
        if (vehicleCode) {
          if (vehicleCode._vehicleState == MSVehicleController.ControlState.isPlayer) {
            if (vehicleCode.currentGear > -1 && !vehicleCode.changinGears) {
              vehicleCode.StartCoroutine("ChangeGears", vehicleCode.currentGear - 1);
            }
          }
        }
      }
    }

    //lights
    void Mobile_FlashesRightAlertButton() {
      if (controls.enable_flashesRightAlert_Button_Mobile) {
        if (vehicleCode) {
          if (vehicleCode._vehicleState == MSVehicleController.ControlState.isPlayer) {
            if (!vehicleCode.alertOn) {
              if (!vehicleCode.rightBlinkersOn) {
                vehicleCode.rightBlinkersOn = true;
                vehicleCode.leftBlinkersOn = false;
                vehicleCode.disableBlinkers1 = true;
              } else if (vehicleCode.rightBlinkersOn) {
                vehicleCode.rightBlinkersOn = false;
                vehicleCode.leftBlinkersOn = false;
                vehicleCode.disableBlinkers1 = false;
              }
            }
          }
        }
      }
    }
    void Mobile_FlashesLeftAlertButton() {
      if (controls.enable_flashesLeftAlert_Button_Mobile) {
        if (vehicleCode) {
          if (vehicleCode._vehicleState == MSVehicleController.ControlState.isPlayer) {
            if (!vehicleCode.alertOn) {
              if (!vehicleCode.leftBlinkersOn) {
                vehicleCode.rightBlinkersOn = false;
                vehicleCode.leftBlinkersOn = true;
                vehicleCode.disableBlinkers1 = true;
              } else if (vehicleCode.leftBlinkersOn) {
                vehicleCode.rightBlinkersOn = false;
                vehicleCode.leftBlinkersOn = false;
                vehicleCode.disableBlinkers1 = false;
              }
            }
          }
        }
      }
    }
    void Mobile_MainLightsButton() {
      if (controls.enable_mainLightsInput_Button_Mobile) {
        if (vehicleCode) {
          if (vehicleCode._vehicleState == MSVehicleController.ControlState.isPlayer) {
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
        }
      }
    }
    void Mobile_ExtraLightsButton() {
      if (controls.enable_extraLightsInput_Button_Mobile) {
        if (vehicleCode) {
          if (vehicleCode._vehicleState == MSVehicleController.ControlState.isPlayer) {
            vehicleCode.extraLightsOn = !vehicleCode.extraLightsOn;
          }
        }
      }
    }
    void Mobile_HeadLightsButton() {
      if (controls.enable_headlightsInput_Button_Mobile) {
        if (vehicleCode) {
          if (vehicleCode._vehicleState == MSVehicleController.ControlState.isPlayer) {
            vehicleCode.headlightsOn = !vehicleCode.headlightsOn;
          }
        }
      }
    }
    void Mobile_WarningLightsButton() {
      if (controls.enable_warningLightsInput_Button_Mobile) {
        if (vehicleCode) {
          if (vehicleCode._vehicleState == MSVehicleController.ControlState.isPlayer) {
            if (vehicleCode.alertOn) {
              vehicleCode.alertOn = false;
              vehicleCode.rightBlinkersOn = vehicleCode.leftBlinkersOn = false;
            } else {
              vehicleCode.alertOn = true;
              vehicleCode.rightBlinkersOn = vehicleCode.leftBlinkersOn = true;
            }
          }
        }
      }
    }

    //game
    void Mobile_ReloadSceneButton() {
      if (controls.enable_reloadScene_Button_Mobile) {
        SceneManager.LoadScene(sceneName);
      }
    }
    void Mobile_PauseButton() {
      if (controls.enable_pause_Button_Mobile) {
        pause = !pause;
        if (pause) {
          Time.timeScale = 0.0f;
        } else {
          Time.timeScale = 1.0f;
        }
      }
    }

    //vehicle
    void Mobile_ManualOrAutoGearsButton() {
      if (controls.enable_manualOrAutoGears_Button_Mobile) {
        if (vehicleCode) {
          if (vehicleCode._vehicleState == MSVehicleController.ControlState.isPlayer) {
            vehicleCode.automaticGears = !vehicleCode.automaticGears;
          }
        }
      }
    }
    void Mobile_StartTheVehicleButton() {
      if (controls.enable_startTheVehicle_Button_Mobile) {
        if (vehicleCode) {
          if (vehicleCode._vehicleState == MSVehicleController.ControlState.isPlayer) {
            if (vehicleCode.youCanCall) {
              if (vehicleCode.theEngineIsRunning) {
                vehicleCode.StartCoroutine(nameof(vehicleCode.StartEngineCoroutine), false);
              } else {
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
        }
      }
    }
    void Mobile_EnterAndExitButton() {
      if (!enterAndExitBoolMobile && controls.enable_enterEndExit_Button_Mobile) {
        enterAndExitBoolMobile = true;
      }
    }
    void Mobile_HornButton() {
      if (controls.enable_hornInput_Button_Mobile) {
        if (vehicleCode) {
          if (vehicleCode._vehicleState == MSVehicleController.ControlState.isPlayer) {
            vehicleCode.hornIsOn = true;
          }
        }
      }
    }
    void Mobile_HandBrakeButton() {
      if (controls.enable_handBrakeInput_Button_Mobile) {
        if (vehicleCode) {
          if (vehicleCode._vehicleState == MSVehicleController.ControlState.isPlayer) {
            vehicleCode.handBrakeTrue = !vehicleCode.handBrakeTrue;
          }
        }
      }
    }

    void Mobile_ChangeSuspensionHeightInputButton() {
      if (controls.enable_changeSuspensionHeight_Button_Mobile) {
        if (vehicleCode._vehicleState == MSVehicleController.ControlState.isPlayer) {
          if (vehicleCode._suspension.vehicleCustomHeights.Length > 0) {
            if (vehicleCode._suspension.indexCustomSuspensionHeight < (vehicleCode._suspension.vehicleCustomHeights.Length - 1)) {
              vehicleCode._suspension.indexCustomSuspensionHeight++;
            } else if (vehicleCode._suspension.indexCustomSuspensionHeight >= (vehicleCode._suspension.vehicleCustomHeights.Length - 1)) {
              vehicleCode._suspension.indexCustomSuspensionHeight = 0;
            }
          }
        }
      }
    }
    void Mobile_NitroMobileButtonInputButton() {
      if (vehicleCode._additionalFeatures.useNitro) {
        vehicleCode._additionalFeatures.nitroIsTrueInput = !vehicleCode._additionalFeatures.nitroIsTrueInput;
      }
    }
    #endregion
    #endregion
  }
}
