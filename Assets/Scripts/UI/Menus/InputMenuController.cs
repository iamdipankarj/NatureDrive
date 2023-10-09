using System;
using UnityEngine;
using UnityEngine.UI;

namespace Solace {
  public class InputMenuController : MonoBehaviour {
    public Toggle invertMouseXToggle;
    public Toggle invertMouseYToggle;
    public Slider xSensitivitySlider;
    public Slider ySensitivitySlider;
    public Slider joyStickSensitivitySlider;
    public Toggle vibrationToggle;

    void Start() {
      invertMouseXToggle.isOn = SettingsManager.instance.GetInvertXAxisEnabled();
      invertMouseYToggle.isOn = SettingsManager.instance.GetInvertYAxisEnabled();
      xSensitivitySlider.value = SettingsManager.instance.GetMouseXSensitivity();
      ySensitivitySlider.value = SettingsManager.instance.GetMouseYSensitivity();
      joyStickSensitivitySlider.value = SettingsManager.instance.GetJoystickSensitivity();
      vibrationToggle.isOn = SettingsManager.instance.GetVibrationEnabled();
    }

    private void OnVibrationChanged(bool value) {
      SettingsManager.instance.SetVibrationEnabled(value);
    }

    private void OnXInvertChanged(bool value) {
      SettingsManager.instance.SetInvertXAxisEnabled(value);
    }

    private void OnYInvertChanged(bool value) {
      SettingsManager.instance.SetInvertYAxisEnabled(value);
    }

    private void OnXSensitivityChanged(float value) {
      SettingsManager.instance.SetMouseXSensitivity((int)value);
    }

    private void OnYSensitivityChanged(float value) {
      SettingsManager.instance.SetMouseYSensitivity((int)value);
    }

    private void OnJoystuckSensivityChanged(float value) {
      SettingsManager.instance.SetJoystickSensitivity((int)value);
    }

    private void OnEnable() {
      invertMouseXToggle.onValueChanged.AddListener(OnXInvertChanged);
      invertMouseYToggle.onValueChanged.AddListener(OnYInvertChanged);
      xSensitivitySlider.onValueChanged.AddListener(OnXSensitivityChanged);
      ySensitivitySlider.onValueChanged.AddListener(OnYSensitivityChanged);
      joyStickSensitivitySlider.onValueChanged.AddListener(OnJoystuckSensivityChanged);
      vibrationToggle.onValueChanged.AddListener(OnVibrationChanged);
    }

    private void OnDisable() {
      invertMouseXToggle.onValueChanged.RemoveListener(OnXInvertChanged);
      invertMouseYToggle.onValueChanged.RemoveListener(OnYInvertChanged);
      xSensitivitySlider.onValueChanged.RemoveListener(OnXSensitivityChanged);
      ySensitivitySlider.onValueChanged.RemoveListener(OnYSensitivityChanged);
      joyStickSensitivitySlider.onValueChanged.RemoveListener(OnJoystuckSensivityChanged);
      vibrationToggle.onValueChanged.RemoveListener(OnVibrationChanged);
    }
  }
}
