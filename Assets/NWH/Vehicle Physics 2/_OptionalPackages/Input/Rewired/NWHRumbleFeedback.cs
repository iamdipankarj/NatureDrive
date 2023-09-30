using UnityEngine;
using Solace;
using System.Collections;
using Codice.Client.BaseCommands;

namespace NWH.VehiclePhysics2.Input {
  public class NWHRumbleFeedback : RewiredRumbleProviderBase {
    public VehicleController vehicleController;
    private bool isColliding = false;
    private Coroutine delayedCollision;

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

    void Update() {
      float speed = vehicleController.Speed / 50.8f;
      if (!isColliding) {
        base.SetStaticVibration(speed);
      }
    }
  }
}
