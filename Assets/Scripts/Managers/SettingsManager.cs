using System.Collections.Generic;
using UnityEngine;

namespace Solace {
  public enum WindowType {
    WINDOWED = 1,
    FULL_SCREEN = 2,
    BORDERLESS = 3
  }

  public enum QualityIndex {
    VERY_LOW = 0,
    LOW = 1,
    MEDIUM = 2,
    HIGH = 3,
    VERY_HIGH = 4,
    ULTRA = 5
  }

  public enum Difficulty {
    EASY = 1,
    REGULAR = 2,
    HARD = 3
  }

  public enum AntiAliasingLevel {
    NONE = 0,
    X2 = 2,
    X4 = 4,
    X8 = 8
  }

  public class SettingsManager : MonoBehaviour {
    public static SettingsManager instance;

    public delegate void ResolutionAction(List<string> options);
    public static event ResolutionAction DidReceiveResolutions;

    // Default values
    private const int DEFAULT_MOUSE_SENSITIVITY = 50;
    private const int DEFAULT_BRIGHTNESS = 50;
    private const float DEFAULT_MASTER_VOLUME = 1f;
    private const float DEFAULT_MUSIC_VOLUME = 0.5f;
    private const float DEFAULT_SFX_VOLUME = 0.5f;

    // Game Settings
    private const string DIFFICULTY_KEY = "atom_difficulty";
    private const string HUD_KEY = "hud_enabled_key";

    // Graphics Settings
    private const string MOTION_BLUR_KEY = "motion_blur_enabled_key";
    private const string COLOR_BLIND_KEY = "color_blind_mode_key";
    private const string VSYNC_KEY = "vsync_key";
    private const string FPS_KEY = "fps_key";
    private const string ANTI_ALIAS_KEY = "anti_alias_key";
    private const string BLOOM_KEY = "bloom_key";
    private const string BRIGHTNESS_KEY = "brightness_key";
    private const string BILINEAR_FILTER_KEY = "bilinear_filter_key";
    private const string RESOLUTION_KEY = "resolution_key";

    // Control Settings
    private const string VIBRATION_KEY = "vibration_enabled_key";
    private const string INVERT_Y_AXIS_KEY = "invert_y_axis_enabled_key";
    private const string MOUSE_SENSITIVITY_KEY = "mouse_sensitivity_key";

    // Audio Settings
    private const string MASTER_VOLUME_KEY = "master_volume_key";
    private const string MUSIC_VOLUME_KEY = "music_volume_key";
    private const string SFX_VOLUME_KEY = "sfx_volume_key";

    // Language Settings
    private const string LANGUAGE_KEY = "selected_locale_key";

    private readonly List<string> colorBlindModes = new() {
      "Normal",
      "Protanopia",
      "Protanomaly",
      "Deuteranopia",
      "Deuteranomaly",
      "Tritanopia",
      "Tritanomaly",
      "Achromatopsia",
      "Achromatomaly"
     };

    private void Awake() {
      if (instance == null) {
        instance = this;
      }
      else {
        Destroy(gameObject);
      }
      DontDestroyOnLoad(gameObject);
    }

    void Start() {
      DidReceiveResolutions?.Invoke(GetAvailableResolutions(Screen.resolutions));
    }

    public List<string> GetColorBlindModes() {
      return colorBlindModes;
    }

    public void SetBilinearFilteringEnabled(bool enabled) {
      PlayerPrefs.SetInt(BILINEAR_FILTER_KEY, enabled ? 1 : 0);
    }

    public bool GetBilinearFilteringEnabled() {
      if (PlayerPrefs.HasKey(BILINEAR_FILTER_KEY)) {
        return PlayerPrefs.GetInt(BILINEAR_FILTER_KEY) == 1;
      }
      return false;
    }

    public void SetLanguage(string code) {
      PlayerPrefs.SetString(LANGUAGE_KEY, code);
    }

    public string GetLanguage() {
      return PlayerPrefs.GetString(LANGUAGE_KEY, "en");
    }

    public void SetBloomEnabled(bool enabled) {
      PlayerPrefs.SetInt(BLOOM_KEY, enabled ? 1 : 0);
    }

    public bool GetBloomEnabled() {
      if (PlayerPrefs.HasKey(BLOOM_KEY)) {
        return PlayerPrefs.GetInt(BLOOM_KEY) == 1;
      }
      return false;
    }

    public void SetMasterVolume(float amount) {
      PlayerPrefs.SetFloat(MASTER_VOLUME_KEY, amount);
    }

    public float GetMasterVolume() {
      if (PlayerPrefs.HasKey(MASTER_VOLUME_KEY)) {
        return PlayerPrefs.GetFloat(MASTER_VOLUME_KEY);
      }
      return DEFAULT_MASTER_VOLUME;
    }

    public void SetMusicVolume(float amount) {
      PlayerPrefs.SetFloat(MUSIC_VOLUME_KEY, amount);
    }

    public float GetMusicVolume() {
      if (PlayerPrefs.HasKey(MUSIC_VOLUME_KEY)) {
        return PlayerPrefs.GetFloat(MUSIC_VOLUME_KEY);
      }
      return DEFAULT_MUSIC_VOLUME;
    }

    public void SetSFXVolume(float amount) {
      PlayerPrefs.SetFloat(SFX_VOLUME_KEY, amount);
    }

    public float GetSFXVolume() {
      if (PlayerPrefs.HasKey(SFX_VOLUME_KEY)) {
        return PlayerPrefs.GetFloat(SFX_VOLUME_KEY);
      }
      return DEFAULT_SFX_VOLUME;
    }

    public void SetBrightness(int amount) {
      PlayerPrefs.SetInt(BRIGHTNESS_KEY, amount);
    }

    public int GetBrightness() {
      if (PlayerPrefs.HasKey(BRIGHTNESS_KEY)) {
        return PlayerPrefs.GetInt(BRIGHTNESS_KEY);
      }
      return DEFAULT_BRIGHTNESS;
    }

    public void SetMouseSensitivity(int amount) {
      PlayerPrefs.SetInt(MOUSE_SENSITIVITY_KEY, amount);
    }

    public int GetMouseSensitivity() {
      if (PlayerPrefs.HasKey(MOUSE_SENSITIVITY_KEY)) {
        return PlayerPrefs.GetInt(MOUSE_SENSITIVITY_KEY);
      }
      return DEFAULT_MOUSE_SENSITIVITY;
    }

    public float GetNormalizedMouseSensitivity() {
      return GetMouseSensitivity() / 100;
    }

    public void SetAntiAliasLevel(AntiAliasingLevel level) {
      PlayerPrefs.SetInt(ANTI_ALIAS_KEY, (int)level);
    }

    public AntiAliasingLevel GetAntiAliasingLevel() {
      if (PlayerPrefs.HasKey(ANTI_ALIAS_KEY)) {
        return (AntiAliasingLevel)PlayerPrefs.GetInt(ANTI_ALIAS_KEY);
      }
      return AntiAliasingLevel.X2;
    }

    public void SetFPSLimit(int fps) {
      if (fps == 60) {
        QualitySettings.vSyncCount = 1;
        Application.targetFrameRate = fps;
        PlayerPrefs.SetInt(FPS_KEY, fps);
      }
      else if (fps == 30) {
        QualitySettings.vSyncCount = 2;
        Application.targetFrameRate = fps;
        PlayerPrefs.SetInt(FPS_KEY, fps);
      }
      else if (fps == -1) { // Unlimted framerate
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = -1;
        PlayerPrefs.SetInt(FPS_KEY, fps);
      }
    }

    public int GetFPSLimit() {
      if (PlayerPrefs.HasKey(FPS_KEY)) {
        return PlayerPrefs.GetInt(FPS_KEY);
      }
      return 60;
    }

    public void SetVsyncEnabled(bool enabled) {
      PlayerPrefs.SetInt(VSYNC_KEY, enabled ? 1 : 0);
      QualitySettings.vSyncCount = enabled ? 1 : 0;
    }

    public bool GetVsyncEnabled() {
      if (PlayerPrefs.HasKey(VSYNC_KEY)) {
        return PlayerPrefs.GetInt(VSYNC_KEY) == 1;
      }
      return false;
    }

    public void SetMotionBlurEnabled(bool enabled) {
      PlayerPrefs.SetInt(MOTION_BLUR_KEY, enabled ? 1 : 0);
    }

    public bool GetMotionBlurEnabled() {
      if (PlayerPrefs.HasKey(MOTION_BLUR_KEY)) {
        return PlayerPrefs.GetInt(MOTION_BLUR_KEY) == 1;
      }
      return false;
    }

    public void SetVibrationEnabled(bool enabled) {
      PlayerPrefs.SetInt(VIBRATION_KEY, enabled ? 1 : 0);
    }

    public bool GetVibrationEnabled() {
      if (PlayerPrefs.HasKey(VIBRATION_KEY)) {
        return PlayerPrefs.GetInt(VIBRATION_KEY) == 1;
      }
      return true;
    }

    public void SetHUDEnabled(bool enabled) {
      PlayerPrefs.SetInt(HUD_KEY, enabled ? 1 : 0);
    }

    public bool GetHUDEnabled() {
      if (PlayerPrefs.HasKey(HUD_KEY)) {
        return PlayerPrefs.GetInt(HUD_KEY) == 1;
      }
      return false;
    }

    public void SetInvertYAxisEnabled(bool enabled) {
      PlayerPrefs.SetInt(INVERT_Y_AXIS_KEY, enabled ? 1 : 0);
    }

    public bool GetInvertYAxisEnabled() {
      if (PlayerPrefs.HasKey(INVERT_Y_AXIS_KEY)) {
        return PlayerPrefs.GetInt(INVERT_Y_AXIS_KEY) == 1;
      }
      return false;
    }

    public void SetColorBlindMode(ColorBlindMode value) {
      PlayerPrefs.SetInt(COLOR_BLIND_KEY, (int)value);
    }

    public ColorBlindMode GetColorBlindMode() {
      if (PlayerPrefs.HasKey(COLOR_BLIND_KEY)) {
        return (ColorBlindMode)PlayerPrefs.GetInt(COLOR_BLIND_KEY);
      }
      return ColorBlindMode.Normal;
    }

    public void SetDifficulty(Difficulty difficulty) {
      PlayerPrefs.SetInt(DIFFICULTY_KEY, (int)difficulty);
    }

    public Difficulty GetDifficulty() {
      if (PlayerPrefs.HasKey(DIFFICULTY_KEY)) {
        return (Difficulty)PlayerPrefs.GetInt(DIFFICULTY_KEY);
      }
      return Difficulty.REGULAR;
    }

    public void SetResolution(string resolution) {
      PlayerPrefs.SetString(RESOLUTION_KEY, resolution);
    }

    public string GetResolution() {
      if (PlayerPrefs.HasKey(RESOLUTION_KEY)) {
        return PlayerPrefs.GetString(RESOLUTION_KEY);
      }
      return GetDefaultResolution();
    }

    public void SetResolution(int width, int height) {
      Screen.SetResolution(width, height, GetCurrentWindowType(), new RefreshRate()
      {
        numerator = Screen.currentResolution.refreshRateRatio.numerator,
        denominator = Screen.currentResolution.refreshRateRatio.denominator
      });
    }

    private string GetDefaultResolution() {
      Resolution res = Screen.currentResolution;
      return $"{res.width}x{res.height}";
    }

    public List<string> GetAvailableResolutions(Resolution[] resolutions) {
      List<string> options = new();
      foreach (var res in resolutions) {
        options.Add($"{res.width}x{res.height}");
      }
      return options;
    }

    public void SetDefaultResolution() {
      Resolution currentResolution = Screen.currentResolution;
      SetResolution(currentResolution.width, currentResolution.height);
    }

    public void SetQuality(QualityIndex qualityIndex) {
      QualitySettings.SetQualityLevel((int)qualityIndex, true);
    }

    public FullScreenMode GetCurrentWindowType() {
      return Screen.fullScreenMode;
    }

    public void SetWindowType(WindowType windowType) {
      if (windowType == WindowType.WINDOWED) {
        Screen.fullScreenMode = FullScreenMode.Windowed;
      }
      else if (windowType == WindowType.FULL_SCREEN) {
        Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
      }
      else if (windowType == WindowType.BORDERLESS) {
        Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
      }
    }

    public string GetGraphicsDeviceName() {
      return SystemInfo.graphicsDeviceName;
    }

    public void SaveAll() {
      PlayerPrefs.Save();
    }

    void OnDestroy() {
      PlayerPrefs.Save();
    }
  }
}
