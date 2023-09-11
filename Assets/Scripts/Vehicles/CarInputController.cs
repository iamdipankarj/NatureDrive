using UnityEngine;

namespace Solace {
  public class CarInputController : MonoBehaviour {
    [HideInInspector]
    public bool isAcceleratingForward;
    [HideInInspector]
    public float forwardAccelerateDelta;

    [HideInInspector]
    public bool isAcceleratingBackward;
    [HideInInspector]
    public float backwardAccelerateDelta;

    [HideInInspector]
    public bool isTurningLeft;
    [HideInInspector]
    public bool isTurningRight;
    [HideInInspector]
    public float steeringDelta;

    [HideInInspector]
    public bool isPressingHandbrake;
    [HideInInspector]
    public bool isReleasingHandbrake;

    private void OnHandBrake(bool isPressing) {
      if (isPressing) {
        isPressingHandbrake = true;
        isReleasingHandbrake = false;
      }
      else {
        isPressingHandbrake = false;
        isReleasingHandbrake = true;
      }
    }

    private void OnSteer(float delta) {
      if (delta < 0f) {
        isTurningLeft = true;
        isTurningRight = false;
      } else if (delta > 0f) {
        isTurningRight = true;
        isTurningLeft = false;
      } else if (delta.Equals(0f)) {
        isTurningRight = false;
        isTurningLeft = false;
      }
      steeringDelta = Mathf.Abs(delta);
    }

    private void OnAccelerateForward(float delta) {
      isAcceleratingForward = delta > 0f;
      forwardAccelerateDelta = delta;
    }

    private void OnAccelerateBackward(float delta) {
      isAcceleratingBackward = delta > 0f;
      backwardAccelerateDelta = delta;
    }

    private void Start() {
      
    }

    private void OnEnable() {
      InputManager.DidAccelerate += OnAccelerateForward;
      InputManager.DidReverse += OnAccelerateBackward;
      InputManager.DidSteer += OnSteer;
      InputManager.DidUseHandBrake += OnHandBrake;
    }

    private void OnApplicationQuit() {
      LogitechGSDK.LogiSteeringShutdown();
    }

    private void OnDisable() {
      InputManager.DidAccelerate -= OnAccelerateForward;
      InputManager.DidReverse -= OnAccelerateBackward;
      InputManager.DidSteer -= OnSteer;
      InputManager.DidUseHandBrake -= OnHandBrake;
    }

    void Update() {
      //isAcceleratingForward = Input.GetKey(KeyCode.W);
      //isAcceleratingBackward = Input.GetKey(KeyCode.S);
      //isTurningLeft = Input.GetKey(KeyCode.A);
      //isTurningRight = Input.GetKey(KeyCode.D);
      //isPressingHandbrake = Input.GetKey(KeyCode.Space);
      //isReleasingHandbrake = Input.GetKeyUp(KeyCode.Space);
    }
  }
}
