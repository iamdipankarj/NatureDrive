using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Solace {
  [RequireComponent(typeof(CharacterController))]
  public class PlayerController : MonoBehaviour {
    private const float walkingSpeed = 7.5f;
    private const float runningSpeed = 11.5f;
    private const float jumpSpeed = 8.0f;
    private const float gravity = 20.0f;

    private Vector2 moveDelta;

    public bool isGrounded;
    public bool isRunning;
    private bool isJumping;

    private Transform cameraTransform;

    public Vector2 velocity = Vector2.zero;
    private const float pushPower = 10.0f;

    CharacterController characterController;
    public Vector3 moveDirection = Vector3.zero;

    private void OnPlayerJump(bool didJump) {
      isJumping = didJump;
    }

    private void OnPlayerSprint(bool isSprinting) {
      isRunning = isSprinting;
    }

    private void OnPlayerMove(Vector2 delta) {
      moveDelta = delta;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit) {
      Rigidbody body = hit.collider.attachedRigidbody;

      // no rigidbody
      if (body == null || body.isKinematic)
        return;

      // We dont want to push objects below us
      if (hit.moveDirection.y < -0.3f)
        return;

      // Calculate push direction from move direction,
      // we only push objects to the sides never up and down
      Vector3 pushDir = new(hit.moveDirection.x, 0, hit.moveDirection.z);

      // If you know how fast your character is trying to move,
      // then you can also multiply the push velocity by that.

      // Apply the push
      body.velocity = pushPower * pushDir;
    }

    void Update() {
      // We are grounded, so recalculate move direction based on axes
      Vector3 forward = transform.TransformDirection(Vector3.forward);
      Vector3 right = transform.TransformDirection(Vector3.right);

      // Press Left Shift to run
      float curSpeedX = (isRunning ? runningSpeed : walkingSpeed) * moveDelta.y;
      float curSpeedY = (isRunning ? runningSpeed : walkingSpeed) * moveDelta.x;
      float movementDirectionY = moveDirection.y;
      moveDirection = (forward * curSpeedX) + (right * curSpeedY);

      isGrounded = characterController.isGrounded;

      velocity = characterController.velocity;

      if (isJumping && characterController.isGrounded) {
        moveDirection.y = jumpSpeed;
      } else {
        moveDirection.y = movementDirectionY;
      }

      // Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below
      // when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
      // as an acceleration (ms^-2)
      if (!characterController.isGrounded) {
        moveDirection.y -= gravity * Time.deltaTime;
      }

      // Move the controller
      characterController.Move(moveDirection * Time.deltaTime);

      // Player rotation
      transform.rotation = Quaternion.Euler(0f, cameraTransform.rotation.eulerAngles.y, 0f);
    }

    private void OnEnable() {
      InputManager.DidMove += OnPlayerMove;
      InputManager.DidSprint += OnPlayerSprint;
      InputManager.DidJump += OnPlayerJump;
    }

    private void OnDisable() {
      InputManager.DidMove -= OnPlayerMove;
      InputManager.DidSprint -= OnPlayerSprint;
      InputManager.DidJump -= OnPlayerJump;
    }

    void Start() {
      CursorManager.LockCursor();
      characterController = GetComponent<CharacterController>();
      cameraTransform = Camera.main.transform;
    }
  }
}
