using UnityEngine;
using UnityEngine.UI;
using LastUI;
using Unity.VisualScripting;
using System;

namespace Solace {
  public class DisplayMenuController : MonoBehaviour {
    public Toggle vsyncToggle;
    public Slider framerateSlider;
    public Slider brightnessSlider;
    public HorizontalSelector resolutionSelector;
    public HorizontalSelector displayModeSelector;
    public HorizontalSelector antiAliasSelector;

    // Resolution stuff
    private Resolution[] resolutions;
    private int currentScreenWidth;
    private int currentScreenHeight;

    private void OnAntiAliasChanged(int index) {
      if (index == 0) {
        QualitySettings.antiAliasing = (int)AntiAliasingLevel.NONE;
      } else if (index == 1) {
        QualitySettings.antiAliasing = (int)AntiAliasingLevel.X2;
      } else if (index == 2) {
        QualitySettings.antiAliasing = (int)AntiAliasingLevel.X4;
      } else if (index == 3) {
        QualitySettings.antiAliasing = (int)AntiAliasingLevel.X8;
      }
      SettingsManager.instance.SetAntiAliasLevel((AntiAliasingLevel)index);
    }

    private void OnVsyncChanged(bool isOn) {
      SettingsManager.instance.SetVsyncEnabled(isOn);
    }

    private void OnBrightnessChanged(float value) {
      SettingsManager.instance.SetBrightness((int)value);
    }

    private void OnFramerateChanged(float value) {
      SettingsManager.instance.SetFPSLimit((int)value);
    }

    private void PrepareResolutionDropdown() {
      // Screen width and height
      currentScreenWidth = Screen.currentResolution.width;
      currentScreenHeight = Screen.currentResolution.height;

      // Store available resolutions
      resolutions = Screen.resolutions;

      // Reset drop down
      resolutionSelector.Clear();

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
        resolutionSelector.Add(new($"{res.width} x {res.height}"));
        if (res.width == width && res.height == height) {
          selectedIndex = i;
        }
      }
      resolutionSelector.index = selectedIndex;
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

    private void OnResolutionChanged(int resolutionIndex) {
      Resolution resolution = resolutions[resolutionIndex];
      SettingsManager.instance.SetResolution(resolution.width, resolution.height);
      UpdateResolution(resolution.width, resolution.height);
    }

    private void UpdateResolution(int width, int height) {
      Screen.SetResolution(width, height, Screen.fullScreenMode, new RefreshRate() {
        numerator = Screen.currentResolution.refreshRateRatio.numerator,
        denominator = Screen.currentResolution.refreshRateRatio.denominator
      });
    }

    private void Start() {
      PrepareResolutionDropdown();
      Debug.Log((int)SettingsManager.instance.GetAntiAliasingLevel());
      antiAliasSelector.index = (int)SettingsManager.instance.GetAntiAliasingLevel();
      displayModeSelector.index = SettingsManager.instance.GetDisplayMode();
      vsyncToggle.isOn = SettingsManager.instance.GetVsyncEnabled();
      brightnessSlider.value = SettingsManager.instance.GetBrightness();
      framerateSlider.value = SettingsManager.instance.GetFPSLimit();
    }

    private void OnEnable() {
      brightnessSlider.onValueChanged.AddListener(OnBrightnessChanged);
      framerateSlider.onValueChanged.AddListener(OnFramerateChanged);
      vsyncToggle.onValueChanged.AddListener(OnVsyncChanged);
      displayModeSelector.OnValueChanged += OnDisplayModeChanged;
      resolutionSelector.OnValueChanged += OnResolutionChanged;
      antiAliasSelector.OnValueChanged += OnAntiAliasChanged;
    }

    private void OnDisable() {
      brightnessSlider.onValueChanged.RemoveListener(OnBrightnessChanged);
      framerateSlider.onValueChanged.RemoveListener(OnFramerateChanged);
      vsyncToggle.onValueChanged.RemoveListener(OnVsyncChanged);
      displayModeSelector.OnValueChanged -= OnDisplayModeChanged;
      resolutionSelector.OnValueChanged -= OnResolutionChanged;
      antiAliasSelector.OnValueChanged -= OnAntiAliasChanged;
    }
  }
}
