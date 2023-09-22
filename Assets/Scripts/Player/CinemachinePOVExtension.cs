using UnityEngine;
using Cinemachine;

namespace Solace {
  public class CinemachinePOVExtension : CinemachineExtension {
    public CurveControlledBob motionBob = new();
    public LerpControlledBob jumpAndLandingBob = new();
    public PlayerController playerController;
    public float strideInterval = 4f;
    public float runningStrideLengthen = 0.722f;

    private void Start() {
      motionBob.Setup(base.VirtualCamera, strideInterval);
    }

    /// <summary>
    /// Called after the end of each stage.
    /// Here we override the orientation of the camera 
    /// </summary>
    /// <param name="vcam">Virtual camera</param>
    /// <param name="stage">What stage the pipeline is currently on</param>
    /// <param name="state">The output of the Cinemachine engine for a specific virtual camera. We won't be using this. </param>
    /// <param name="deltaTime">Current delta time. We won't be using this.</param>
    protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime) {
      if (Application.IsPlaying(gameObject) && vcam.Follow) {
        if (stage == CinemachineCore.Stage.Aim) {
          Vector3 newCameraPosition;
          if (playerController.velocity.magnitude > 1.5f && playerController.isGrounded) {
            newCameraPosition = motionBob.DoHeadBob(playerController.velocity.magnitude * (playerController.isRunning ? runningStrideLengthen : 1f));
          } else {
            newCameraPosition = vcam.transform.localPosition;
          }

          Debug.Log(newCameraPosition);

          //vcam.transform.position = newCameraPosition;
          state.RawPosition = new Vector3(state.RawPosition.x + newCameraPosition.x, state.RawPosition.y + newCameraPosition.y, state.RawPosition.z + newCameraPosition.z);

          //// Get input system mouse delta values and add them to the starting rotation along with speed
          //Vector2 deltaInput = inputManager.GetMouseDelta();
          //startingRotation.x += deltaInput.x * verticalSpeed * Time.deltaTime;
          //startingRotation.y += deltaInput.y * horizontalSpeed * Time.deltaTime;
          //// Clamp the values to make sure the player can't keep looking up (it would rotate over their head)
          //startingRotation.y = Mathf.Clamp(startingRotation.y, -clampAngle, clampAngle);
          //// Rotate the orientation of the camera to match the new delta values.
          //// If you look sideways, you need to rotate it on the Y axis, 
          //// thus put startingRotation.y in the x value for Euler calculation.
          //// We add a negative in front of startingRotation.y to invert the axis.
          //state.RawOrientation = Quaternion.Euler(-startingRotation.y, startingRotation.x, 0f);
        }
      }
    }
  }

}
