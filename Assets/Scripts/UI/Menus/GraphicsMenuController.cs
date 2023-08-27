using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Solace {
  public class GraphicsMenuController : MenuController {
    public Slider brightnessSlider;
    public TMP_Dropdown qualityDropdown;
    public TMP_Dropdown displayModeDropdown;
    public TMP_Dropdown resolutionDropdown;
    public Toggle vsyncToggle;
    public Slider framerateSlider;
    public Toggle motionBlurToggle;

    // Resolution stuff
    private Resolution[] resolutions;
    private float currentRefreshRate = 0;
    private int currentScreenWidth;
    private int currentScreenHeight;

    // Display mode
    private List<string> displayModeOptions;

    // Quality
    private List<string> qualityOptions;

    private void OnEnable() {
      brightnessSlider.onValueChanged.AddListener(OnBrightnessChanged);
      framerateSlider.onValueChanged.AddListener(OnFramerateChanged);
      resolutionDropdown.onValueChanged.AddListener(OnResolutionChanged);
      displayModeDropdown.onValueChanged.AddListener(OnDisplayModeChanged);
      qualityDropdown.onValueChanged.AddListener(OnQualityChanged);
      vsyncToggle.onValueChanged.AddListener(OnVsyncChanged);
    }

    private void OnVsyncChanged(bool isOn) {
      SettingsManager.instance.SetVsyncEnabled(isOn);
    }

    private void PrepareVsyncSettings() {
      vsyncToggle.isOn = SettingsManager.instance.GetVsyncEnabled();
    }

    private void PrepareQualitySettingsDropdown() {
      qualityOptions = new() {
        "Very Low", "Low", "Medium", "High", "Very High", "Ultra"
      };
      QualityIndex preferedIndex = SettingsManager.instance.GetQuality();
      qualityDropdown.ClearOptions();
      qualityDropdown.AddOptions(qualityOptions);
      qualityDropdown.value = (int)preferedIndex;
      qualityDropdown.RefreshShownValue();
    }

    private void OnQualityChanged(int qualityIndex) {
      QualityIndex preferedIndex = (QualityIndex)qualityIndex;
      SettingsManager.instance.SetQuality(preferedIndex);
    }

    private void OnDisplayModeChanged(int modeIndex) {
      if (modeIndex == 0) {
        Screen.fullScreenMode = FullScreenMode.Windowed;
      } else if (modeIndex == 1) {
        Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
      } else if (modeIndex == 2) {
        Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
      }
      SettingsManager.instance.SetDisplayMode(modeIndex);
    }

    private void PrepareDisplayModeDropdown() {
      displayModeOptions = new() {
        "Windowed", "Full Screen", "Borderless"
      };
      displayModeDropdown.ClearOptions();
      displayModeDropdown.AddOptions(displayModeOptions);
      int preferedMode = SettingsManager.instance.GetDisplayMode();
      displayModeDropdown.value = preferedMode;
      displayModeDropdown.RefreshShownValue();
    }

    private void PrepareResolutionDropdown() {
      // Screen width and height
      currentScreenWidth = Screen.currentResolution.width;
      currentScreenHeight = Screen.currentResolution.height;

      // Store current refresh rate
      currentRefreshRate = Screen.currentResolution.refreshRateRatio.numerator / Screen.currentResolution.refreshRateRatio.denominator;

      // Store available resolutions
      resolutions = Screen.resolutions;

      // Reset drop down
      resolutionDropdown.ClearOptions();

      // Preferred screen resolution
      var (width, height) = SettingsManager.instance.GetResolution();

      // Auto-select current resolution
      int selectedIndex = 0;

      // Update current resolution if the preferred resolution is different
      if (currentScreenWidth != width && currentScreenHeight != height) {
        UpdateResolution(width, height);
      }

      // Iterate resolutions
      for (int i = 0; i < resolutions.Length; i++) {
        var res = resolutions[i];
        resolutionDropdown.options.Add(new($"{res.width} x {res.height} @ {currentRefreshRate} Hz"));
        if (res.width == width && res.height == height) {
          selectedIndex = i;
        }
      }
      resolutionDropdown.value = selectedIndex;
      resolutionDropdown.RefreshShownValue();
    }

    private void OnResolutionChanged(int resolutionIndex) {
      Resolution resolution = resolutions[resolutionIndex];
      SettingsManager.instance.SetResolution(resolution.width, resolution.height);
      UpdateResolution(resolution.width, resolution.height);
    }

    private void UpdateResolution(int width, int height) {
      Screen.SetResolution(width, height, Screen.fullScreenMode, new RefreshRate()
      {
        numerator = Screen.currentResolution.refreshRateRatio.numerator,
        denominator = Screen.currentResolution.refreshRateRatio.denominator
      });
    }

    private void OnBrightnessChanged(float value) {
      SettingsManager.instance.SetBrightness((int)value);
    }

    private void OnFramerateChanged(float value) {
      SettingsManager.instance.SetFPSLimit((int)value);
    }

    private void OnDisable() {
      brightnessSlider.onValueChanged.RemoveListener(OnBrightnessChanged);
      framerateSlider.onValueChanged.RemoveListener(OnFramerateChanged);
      resolutionDropdown.onValueChanged.RemoveListener(OnResolutionChanged);
      displayModeDropdown.onValueChanged.RemoveListener(OnDisplayModeChanged);
      qualityDropdown.onValueChanged.RemoveListener(OnQualityChanged);
      vsyncToggle.onValueChanged.RemoveListener(OnVsyncChanged);
    }

    void Start() {
      PrepareVsyncSettings();
      PrepareQualitySettingsDropdown();
      PrepareDisplayModeDropdown();
      PrepareResolutionDropdown();
      brightnessSlider.value = SettingsManager.instance.GetBrightness();
      framerateSlider.value = SettingsManager.instance.GetFPSLimit();
    }
  }
}
