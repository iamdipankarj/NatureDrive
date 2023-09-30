using UnityEngine;
using Solace;
using System.Collections;
using Rewired;
using Rewired.ControllerExtensions;

namespace NWH.VehiclePhysics2.Input {
  public class NWHRumbleFeedback : RewiredRumbleProviderBase {
    public VehicleController vehicleController;
    private bool isColliding = false;
    private Coroutine delayedCollision;
    private bool isFlashing = false;

    public override void Awake() {
      base.Awake();
    }

    private void OnEnable() {
      vehicleController.onCollision.AddListener(HandleCollision);
    }

    private void OnDisable() {
      vehicleController.onCollision.RemoveListener(HandleCollision);
      if (delayedCollision != null) {
        StopCoroutine(delayedCollision);
      }
    }

    private IEnumerator CollisionCoroutine() {
      isColliding = true;
      yield return new WaitForSeconds(0.2f);
      isColliding = false;
    }

    private void HandleCollision(Collision collision) {
      delayedCollision = StartCoroutine(CollisionCoroutine());
      float strength = collision.impulse.magnitude / (vehicleController.fixedDeltaTime * vehicleController.vehicleRigidbody.mass * 5f);
      base.SetCollisionVibration(strength);
    }

    private void ToggleFlash() {
      if (isFlashing) {
        StopLightFlash();
      } else {
        StartLightFlash();
      }
      isFlashing = !isFlashing;
    }

    void Update() {
      float speed = vehicleController.Speed / 50.8f;
      if (!isColliding) {
        base.SetStaticVibration(speed);
      }

      if (player.GetButtonDown(RewiredUtils.LeftBlinker)) {
        ToggleFlash();
      }
    }

    private void StartLightFlash() {
      if (ReInput.isReady && GetFirstDS4(player) is DualShock4Extension ds4) {
        ds4.SetLightFlash(0.5f, 0.5f);
      }
    }

    private void StopLightFlash() {
      if (ReInput.isReady && GetFirstDS4(player) is DualShock4Extension ds4) {
        ds4.StopLightFlash();
      }
    }

    private IDualShock4Extension GetFirstDS4(Player player) {
      if (ReInput.isReady) {
        foreach (Joystick j in player.controllers.Joysticks) {
          IDualShock4Extension ds4 = j.GetExtension<IDualShock4Extension>();
          if (ds4 == null) continue;
          return ds4;
        }
        return null;
      }
      return null;
    }
  }
}
