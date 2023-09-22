using Cinemachine;
using System;
using UnityEngine;

namespace Solace {
  [DisallowMultipleComponent]
  public class VehicleManager : MonoBehaviour {
    public static VehicleManager instance;

    // The currently selected vehicle
    public VehicleStandardInput selectedVehicle;

    // Freelook camera in the scene
    public CinemachineFreeLook freeLookCamera;

    public void SetFreeLookFollowAndLookAt(Transform followTarget, Transform lookTarget) {
      freeLookCamera.gameObject.SetActive(true);
      freeLookCamera.Follow = followTarget;
      freeLookCamera.LookAt = lookTarget;
    }

    private void Awake() {
      if (instance == null) {
        instance = this;
      } else {
        Destroy(gameObject);
      }
      DontDestroyOnLoad(gameObject);
    }

    void Start() {
      if (freeLookCamera == null) {
        GameObject freelookObj = GameObject.FindWithTag(TagManager.FREELOOK);
        if (freelookObj.TryGetComponent<CinemachineFreeLook>(out var freelookComp)) {
          freeLookCamera = freelookComp;
        } else {
#if UNITY_EDITOR
          Debug.LogError("Freelook camera not found in the scene");
#endif
        }
      }
    }

    void Update() {

    }
  }
}
