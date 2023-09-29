using Cinemachine;
using System;
using UnityEngine;
using Rewired;

namespace Solace {
  public class VehicleCameraManager : MonoBehaviour {
    [NonSerialized]
    private CinemachineFreeLook freeLookCamera;
    [SerializeField]
    private CinemachineVirtualCamera cockpitCamera;
    [SerializeField]
    private CinemachineVirtualCamera leftWheelCamera;

    // Rewired
    private Player player;

    private CinemachineVirtualCameraBase[] cameras = new CinemachineVirtualCameraBase[3];
    private int selectedCameraIndex = 0;
    private int previousCameraIndex;

    private void Awake() {
      player = ReInput.players.GetPlayer(0);
    }

    void Start() {
      GameObject cameraObj = GameObject.FindWithTag(TagManager.FREELOOK);
      if (cameraObj != null) {
        if (cameraObj.TryGetComponent<CinemachineFreeLook>(out var comp)) {
          freeLookCamera = comp;
        } else {
          Debug.LogError("No Freelook camera found in the scene.");
        }
      }
      cameras[0] = freeLookCamera;
      cameras[1] = cockpitCamera;
      cameras[2] = leftWheelCamera;
    }

    public void SetFreeLookCamera(CinemachineFreeLook cam) {
      cameras[0] = cam;
    }

    private void OnCameraSwitch() {
      previousCameraIndex = selectedCameraIndex;
      if (selectedCameraIndex >= cameras.Length - 1) {
        selectedCameraIndex = 0;
      } else {
        selectedCameraIndex++;
      }
    }

    void Update() {
      if (previousCameraIndex != selectedCameraIndex) {
        SelectCamera();
      }
      if (player.GetButtonDown(RewiredUtils.SwitchCamera)) {
        OnCameraSwitch();
      }
    }

    private void SelectCamera() {
      for (int i = 0; i < cameras.Length; i++) {
        if (cameras[i] != null) {
          if (i == selectedCameraIndex) {
            cameras[i].gameObject.SetActive(true);
          } else {
            cameras[i].gameObject.SetActive(false);
          }
        }
      }
    }

    //private void OnEnable() {
    //  InputManager.DidSwitchCamera += OnCameraSwitch;
    //}

    //private void OnDisable() {
    //  InputManager.DidSwitchCamera -= OnCameraSwitch;
    //}
  }
}
