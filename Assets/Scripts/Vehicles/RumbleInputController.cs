using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Solace {
  public class RumbleInputController : MonoBehaviour {
    private Gamepad pad;
    private CarController car;
    private const float DRIFT_RUMBLE_SCALE = 0.85f;
    private const float STATIC_RUMBLE_SCALE = 0.2f;
    private const float STARTUP_THRESHOLD_SPEED = 20f;
    private float clampedSpeed = 0f;
    private System.Random random;
    private Coroutine RumbleCoroutine;
    private bool isRumbleEnabled = false;

    public float GetRandomFloat(float minimum, float maximum) {
      return (float)(random.NextDouble() * (maximum - minimum) + minimum);
    }

    private (float low, float high) GetFrequency() {
      return (clampedSpeed * DRIFT_RUMBLE_SCALE, clampedSpeed * DRIFT_RUMBLE_SCALE);
    }

    private void SetDriftVibration() {
      pad?.SetMotorSpeeds(GetFrequency().low, GetFrequency().high);
    }

    private void SetStartupVibration() {
      pad?.SetMotorSpeeds(0.0f, 0.8f);
    }

    private void PerformStaticRumble() {
      if (RumbleCoroutine != null) {
        StopCoroutine(RumbleCoroutine);
      }
      RumbleCoroutine = StartCoroutine(StopRumble(GetRandomFloat(0f, 5f)));
    }

    private IEnumerator StopRumble(float duration) {
      SetStaticVibration();
      float elapsedTime = 0f;
      while (elapsedTime < duration) {
        elapsedTime += Time.deltaTime;
        yield return null;
      }
      pad?.SetMotorSpeeds(0f, 0f);
    }

    private void SetStaticVibration() {
      float leftRand = GetRandomFloat(0f, 0.3f);
      float rightRand = GetRandomFloat(0f, 0.2f);
      float low = leftRand * clampedSpeed * 0.8f * STATIC_RUMBLE_SCALE;
      float high = rightRand * clampedSpeed * 0.8f * STATIC_RUMBLE_SCALE;
      pad?.SetMotorSpeeds(low, high);
    }

    private void Start() {
      random = new();
      isRumbleEnabled = SettingsManager.instance.GetVibrationEnabled();
      if (TryGetComponent<CarController>(out var component)) {
        car = component;
      } else {
        Debug.LogWarning("Car Controller not found");
      }
      pad = Gamepad.current;
    }

    private void OnDisable() {
      InputSystem.ResetHaptics();
      if (RumbleCoroutine != null) {
        StopCoroutine(RumbleCoroutine);
      }
    }

    private bool IsSlipping() {
      return car.isDrifting || car.isTractionLocked;
    }

    private bool IsStartingUp() {
      return car.carSpeed > 2f && car.carSpeed <= STARTUP_THRESHOLD_SPEED;
    }

    void FixedUpdate() {
      if (isRumbleEnabled) {
        clampedSpeed = Mathf.Clamp01(Mathf.Abs(car.carSpeed) / car.maxSpeed);
        if (IsStartingUp()) {
          SetStartupVibration();
        } else if (IsSlipping()) {
          SetDriftVibration();
        } else {
          PerformStaticRumble();
        }
      }
    }
  }
}
