using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;

namespace Solace {
  public class RumbleManager : MonoBehaviour {
    public static RumbleManager instance;
    private Gamepad pad;
    private DualShockGamepad dualshockPad;
    private bool isVibrationEnabled;
    private Coroutine StopRumbleAfterTime;

    private void Awake() {
      if (instance == null) {
        instance = this;
      }
      else {
        Destroy(gameObject);
      }
      DontDestroyOnLoad(gameObject);
    }

    public void RumblePulse(float lowFrequency, float highFrequency, float duration) {
      if (pad != null && isVibrationEnabled) {
        pad.SetMotorSpeeds(lowFrequency, highFrequency);
        StopRumbleAfterTime = StartCoroutine(StopRumble(duration, pad));
      }
    }

    private IEnumerator StopRumble(float duration, Gamepad pad) {
      float elapsedTime = 0f;
      while (elapsedTime < duration) {
        elapsedTime += Time.deltaTime;
        yield return null;
      }
      ResetRumble();
    }

    private void ResetRumble() {
      if (pad != null) {
        pad.SetMotorSpeeds(0f, 0f);
      }
    }

    void Start() {
      isVibrationEnabled = SettingsManager.instance.GetVibrationEnabled();
      pad = Gamepad.current;
      if (pad is DualShock4GamepadHID hID) {
        dualshockPad = hID;
      }
    }

    public void SetRedLight() {
      if (dualshockPad != null) {
        if (dualshockPad is DualShock4GamepadHID hID) {
          dualshockPad.SetLightBarColor(Color.red);
        }
      }
    }

    private void OnDestroy() {
      InputSystem.ResetHaptics();
      if (StopRumbleAfterTime != null) {
        StopCoroutine(StopRumbleAfterTime);
      }
    }
  }
}
