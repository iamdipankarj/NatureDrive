using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Solace {
  public class RumbleInputController : MonoBehaviour {
    private Gamepad pad;
    private CarController car;
    private const float driftRumbleScale = 0.85f;
    private const float staticRumbleScale = 0.2f;
    private float clampedSpeed = 0f;
    private System.Random random;
    private Coroutine RumbleCoroutine;
    private bool isRumbleEnabled = false;

    public float GetRandomFloat(float minimum, float maximum) {
      return (float)(random.NextDouble() * (maximum - minimum) + minimum);
    }

    private (float low, float high) GetFrequency() {
      return (clampedSpeed * driftRumbleScale, clampedSpeed * driftRumbleScale);
    }

    private void SetDriftVibration() {
      pad?.SetMotorSpeeds(GetFrequency().low, GetFrequency().high);
    }

    private void PerformStaticRumble() {
      if (RumbleCoroutine != null) {
        StopCoroutine(RumbleCoroutine);
      }
      RumbleCoroutine = StartCoroutine(StopRumble(GetRandomFloat(0f, 10f)));
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
      float leftRand = GetRandomFloat(0f, 0.6f);
      float rightRand = GetRandomFloat(0f, 0.3f);
      float low = leftRand * clampedSpeed * staticRumbleScale;
      float high = rightRand * clampedSpeed * staticRumbleScale;
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

    void FixedUpdate() {
      if (isRumbleEnabled) {
        clampedSpeed = Mathf.Clamp01(Mathf.Abs(car.carSpeed) / car.maxSpeed);
        if (car.isDrifting || car.isTractionLocked) {
          SetDriftVibration();
        } else {
          PerformStaticRumble();
        }
      }
    }
  }
}
