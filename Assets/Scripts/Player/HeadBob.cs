using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Solace {
  public class HeadBob : MonoBehaviour {
    public Camera Camera;
    public CurveControlledBob motionBob = new();
    public LerpControlledBob jumpAndLandingBob = new();
    public PlayerController playerController;
    public float StrideInterval = 4f;
    public float RunningStrideLengthen = 0.722f;

    private void Start() {
      //motionBob.Setup(Camera, StrideInterval);
    }

    private void Update() {
      Vector3 newCameraPosition;
      if (playerController.velocity.magnitude > 1.5f && playerController.isGrounded) {
        Camera.transform.localPosition = motionBob.DoHeadBob(playerController.velocity.magnitude * (playerController.isRunning ? RunningStrideLengthen : 1f));
        newCameraPosition = Camera.transform.localPosition;
      } else {
        newCameraPosition = Camera.transform.localPosition;
      }
      Camera.transform.localPosition = newCameraPosition;
    }
  }
}
