using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Solace {
  public abstract class RumbleStandardInput : MonoBehaviour {
    private Gamepad pad;
    private const float DRIFT_RUMBLE_SCALE = 0.85f;
    private const float staticRumbleScale = 0.09f;
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

    private void SetCollisionVibration() {
      pad?.SetMotorSpeeds(1.0f, 1.0f);
    }

    protected void PlayCollision(int strength) {
      pad?.SetMotorSpeeds(strength, strength);
    }

    private void PerformStaticRumble() {
      if (RumbleCoroutine != null) {
        StopCoroutine(RumbleCoroutine);
      }
      RumbleCoroutine = StartCoroutine(StartRumble(GetRandomFloat(0f, 5f)));
    }

    private IEnumerator StartRumble(float duration) {
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
      float low = leftRand * clampedSpeed * staticRumbleScale;
      float high = rightRand * clampedSpeed * staticRumbleScale;
      pad?.SetMotorSpeeds(low, high);
    }

    protected void RMB_Initialize() {
      random = new();
      isRumbleEnabled = SettingsManager.instance.GetVibrationEnabled();
      pad = Gamepad.current;
    }

    protected void RMB_OnDisable() {
      InputSystem.ResetHaptics();
      if (RumbleCoroutine != null) {
        StopCoroutine(RumbleCoroutine);
      }
    }

    private bool IsStartingUp(float speed) {
      return speed > 2f && speed <= STARTUP_THRESHOLD_SPEED;
    }

    protected void RMB_FixedUpdate(float speed, bool isDrifting, bool isColliding) {
      clampedSpeed = speed;
      if (isRumbleEnabled) {
        if (IsStartingUp(speed)) {
          SetStartupVibration();
        } else if (isColliding) {
          SetCollisionVibration();
        } else if (isDrifting) {
          SetDriftVibration();
        } else {
          PerformStaticRumble();
        }
      }
    }
  }
}
