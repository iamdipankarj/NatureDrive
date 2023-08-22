using System;
using UnityEngine;
using UnityEngine.UI;

namespace Solace {
  public class InputMenuController : MenuController {
    public Toggle isAxisInvertedToggle;
    public Slider mouseSensitivitySlider;

    void Start() {
      isAxisInvertedToggle.isOn = SettingsManager.instance.GetInvertYAxisEnabled();
      mouseSensitivitySlider.value = SettingsManager.instance.GetMouseSensitivity();
    }

    private void OnAxisValueChanged(bool value) {
      SettingsManager.instance.SetInvertYAxisEnabled(value);
    }

    private void OnEnable() {
      isAxisInvertedToggle.onValueChanged.AddListener(OnAxisValueChanged);
      mouseSensitivitySlider.onValueChanged.AddListener(OnSensitivityChanged);
    }

    private void OnSensitivityChanged(float value) {
      SettingsManager.instance.SetMouseSensitivity((int)value);
    }

    private void OnDisable() {
      isAxisInvertedToggle.onValueChanged.RemoveListener(OnAxisValueChanged);
      mouseSensitivitySlider.onValueChanged.RemoveListener(OnSensitivityChanged);
    }
  }
}
