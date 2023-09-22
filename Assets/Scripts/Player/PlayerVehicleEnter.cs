using System;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Assertions;

namespace Solace {
  [DisallowMultipleComponent, RequireComponent(typeof(PlayerController))]
  public class PlayerVehicleEnter : MonoBehaviour {
    private Vector3 reticulePostion = new(0.5f, 0.5f, 0f);
    private const float enterRange = 10f;
    private bool canEnter = false;

    private const string followPointName = "FollowPoint";

    // The vehicle that the user is intending to select.
    private VehicleStandardInput selectionIntent = null;

    private void OnEnterVehicle() {
      if (canEnter) {
        // Disable CM virtual camera
        GameObject.FindWithTag(TagManager.POV_CAMERA).SetActive(false);

        // Disable player capsule mesh
        gameObject.SetActive(false);

        // Enable the freelook camera
        Transform followPoint = selectionIntent.gameObject.transform.Find(followPointName);
#if UNITY_EDITOR
        Assert.IsNotNull(followPoint);
#endif
        VehicleManager.instance.SetFreeLookFollowAndLookAt(selectionIntent.gameObject.transform, followPoint);

        // Enable vehicle controls of selection intent
        if (selectionIntent) {
          selectionIntent.enabled = true;
        }
      }
    }

    private void Update() {
      Ray rayOrigin = Camera.main.ViewportPointToRay(reticulePostion);
      if (Physics.Raycast(rayOrigin, out RaycastHit hitInfo, enterRange)) {
        if (hitInfo.transform.gameObject.CompareTag(TagManager.VEHICLE)) {
          canEnter = true;
          selectionIntent = hitInfo.transform.gameObject.GetComponent<VehicleStandardInput>();
        } else {
          canEnter = false;
          selectionIntent = null;
        }
      }
    }

    private void OnEnable() {
      InputManager.DidEnterVehicle += OnEnterVehicle;
    }

    private void OnDisable() {
      InputManager.DidEnterVehicle -= OnEnterVehicle;
    }
  }
}
