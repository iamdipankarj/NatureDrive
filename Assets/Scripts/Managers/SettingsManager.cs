using System.Collections.Generic;
using UnityEngine;

namespace Solace {
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

    // Display Settings
    private const string VSYNC_KEY = "vsync_key";
    private const string SCREEN_WIDTH_KEY = "screen_width_key";
    private const string SCREEN_HEIGHT_KEY = "screen_height_key";
    private const string DISPLAY_MODE_KEY = "display_mode_key";
    private const string BRIGHTNESS_KEY = "brightness_key";
    private const string ANTI_ALIAS_KEY = "anti_alias_key";
    private const string FPS_KEY = "fps_key";

    // Gameplay settings
    private const string STEERING_ASSIST = "steering_assist_key";
    private const string GAME_TUTORIALS = "game_tutorials_key";
    private const string PHOTO_MODE = "photo_mode_key";

    // Graphics Settings
    private const string QUALITY_KEY = "quality_key";
    private const string SHADOW_QUALITY_KEY = "shadow_quality_key";
    private const string MOTION_BLUR_KEY = "motion_blur_enabled_key";
    private const string COLOR_BLIND_KEY = "color_blind_mode_key";
    private const string SCREEN_SPACE_REFLECTIONS_KEY = "ssr_key";
    private const string BLOOM_KEY = "bloom_key";
    private const string AMBIENT_OCCULUSION_KEY = "ambient_occlusion_key";
    private const string DEPTH_OF_FIELD_KEY = "depth_of_field_key";
    private const string LENS_FLARES_KEY = "lens_flares_key";
    private const string ANISOTROPIC_FILTERING_KEY = "anisotropic_filtering_key";

    // Audio & Language Settings
    private const string MUSIC_VOLUME_KEY = "music_volume_key";
    private const string SFX_VOLUME_KEY = "sfx_volume_key";
    private const string AMBIENT_VOLUME_KEY = "ambient_volume_key";
    private const string LANGUAGE_KEY = "selected_locale_key";

    // Input Settings
    private const string VIBRATION_KEY = "vibration_enabled_key";
    private const string INVERT_Y_AXIS_KEY = "invert_y_axis_enabled_key";
    private const string MOUSE_SENSITIVITY_KEY = "mouse_sensitivity_key";
    private const string JOYSTICK_SENSITIVITY_KEY = "gamepad_sensitivity_key";

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

    public List<string> GetColorBlindModes() {
      return colorBlindModes;
    }

    public void SetResolution(int width, int height) {
      PlayerPrefs.SetInt(SCREEN_WIDTH_KEY, width);
      PlayerPrefs.SetInt(SCREEN_HEIGHT_KEY, height);
    }

    public (int width, int height) GetResolution() {
      return (PlayerPrefs.GetInt(SCREEN_WIDTH_KEY, Screen.currentResolution.width), PlayerPrefs.GetInt(SCREEN_HEIGHT_KEY, Screen.currentResolution.height));
    }

    public void SetLanguage(string code) {
      PlayerPrefs.SetString(LANGUAGE_KEY, code);
    }

    public string GetLanguage() {
      return PlayerPrefs.GetString(LANGUAGE_KEY, "en");
    }

    public void SetScreenSpaceReflections(string code) {
      PlayerPrefs.SetString(SCREEN_SPACE_REFLECTIONS_KEY, code);
    }

    public string GetScreenSpaceReflections() {
      return PlayerPrefs.GetString(SCREEN_SPACE_REFLECTIONS_KEY, "medium");
    }

    public void SetShadowQuality(string code) {
      PlayerPrefs.SetString(SHADOW_QUALITY_KEY, code);
    }

    public string GetShadowQuality() {
      return PlayerPrefs.GetString(SHADOW_QUALITY_KEY, "medium");
    }

    public void SetAmbientOcclusion(bool enabled) {
      PlayerPrefs.SetInt(AMBIENT_OCCULUSION_KEY, enabled ? 1 : 0);
    }

    public bool GetAmbientOcclusion() {
      if (PlayerPrefs.HasKey(AMBIENT_OCCULUSION_KEY)) {
        return PlayerPrefs.GetInt(AMBIENT_OCCULUSION_KEY) == 1;
      }
      return false;
    }

    public void SetLensFlares(bool enabled) {
      PlayerPrefs.SetInt(LENS_FLARES_KEY, enabled ? 1 : 0);
    }

    public bool GetLensFlares() {
      if (PlayerPrefs.HasKey(LENS_FLARES_KEY)) {
        return PlayerPrefs.GetInt(LENS_FLARES_KEY) == 1;
      }
      return false;
    }

    public void SetDepthOfField(bool enabled) {
      PlayerPrefs.SetInt(DEPTH_OF_FIELD_KEY, enabled ? 1 : 0);
    }

    public bool GetDepthOfField() {
      if (PlayerPrefs.HasKey(DEPTH_OF_FIELD_KEY)) {
        return PlayerPrefs.GetInt(DEPTH_OF_FIELD_KEY) == 1;
      }
      return false;
    }

    public void SetAnisoFiltering(bool enabled) {
      PlayerPrefs.SetInt(ANISOTROPIC_FILTERING_KEY, enabled ? 1 : 0);
    }

    public bool GetAnisoFiltering() {
      if (PlayerPrefs.HasKey(ANISOTROPIC_FILTERING_KEY)) {
        return PlayerPrefs.GetInt(ANISOTROPIC_FILTERING_KEY) == 1;
      }
      return false;
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

    public void SetMusicVolume(int amount) {
      PlayerPrefs.SetInt(MUSIC_VOLUME_KEY, amount);
    }

    public int GetMusicVolume() {
      return PlayerPrefs.GetInt(MUSIC_VOLUME_KEY, 5);
    }

    public void SetSFXVolume(int amount) {
      PlayerPrefs.SetInt(SFX_VOLUME_KEY, amount);
    }

    public int GetSFXVolume() {
      return PlayerPrefs.GetInt(SFX_VOLUME_KEY, 5);
    }

    public void SetAmbientVolume(int amount) {
      PlayerPrefs.SetInt(AMBIENT_VOLUME_KEY, amount);
    }

    public int GetAmbientVolume() {
      return PlayerPrefs.GetInt(AMBIENT_VOLUME_KEY, 5);
    }

    public void SetBrightness(int amount) {
      PlayerPrefs.SetInt(BRIGHTNESS_KEY, amount);
    }

    public int GetBrightness() {
      return PlayerPrefs.GetInt(BRIGHTNESS_KEY, 5);
    }

    public void SetJoystickSensitivity(int amount) {
      PlayerPrefs.SetInt(JOYSTICK_SENSITIVITY_KEY, amount);
    }

    public int GetJoystickSensitivity() {
      return PlayerPrefs.GetInt(JOYSTICK_SENSITIVITY_KEY, 5);
    }

    public void SetMouseSensitivity(int amount) {
      PlayerPrefs.SetInt(MOUSE_SENSITIVITY_KEY, amount);
    }

    public int GetMouseSensitivity() {
      return PlayerPrefs.GetInt(MOUSE_SENSITIVITY_KEY, 5);
    }

    public float GetNormalizedMouseSensitivity() {
      return GetMouseSensitivity() / 100;
    }

    public void SetAntiAliasLevel(AntiAliasingLevel level) {
      PlayerPrefs.SetInt(ANTI_ALIAS_KEY, (int)level);
    }

    public AntiAliasingLevel GetAntiAliasingLevel() {
      return (AntiAliasingLevel)PlayerPrefs.GetInt(ANTI_ALIAS_KEY, (int)AntiAliasingLevel.X2);
    }

    public void SetFPSLimit(int fps) {
#if UNITY_EDITOR
      Debug.Log($"Will change framerate to {fps} in production build");
#else
      if (fps >= 99) {
        Application.targetFrameRate = -1;
      } else if (fps >= 60) {
        QualitySettings.vSyncCount = 1;
      } else if (fps >= 30) {
        QualitySettings.vSyncCount = 2;
      } else {
        QualitySettings.vSyncCount = 0;
      }
      Application.targetFrameRate = fps;
#endif
      PlayerPrefs.SetInt(FPS_KEY, fps);
    }

    public void SetSteeringAssist(bool enabled) {
      PlayerPrefs.SetInt(STEERING_ASSIST, enabled ? 1 : 0);
    }

    public bool SetSteeringAssist() {
      if (PlayerPrefs.HasKey(STEERING_ASSIST)) {
        return PlayerPrefs.GetInt(STEERING_ASSIST) == 1;
      }
      return false;
    }

    public void SetGameTutorials(bool enabled) {
      PlayerPrefs.SetInt(GAME_TUTORIALS, enabled ? 1 : 0);
    }

    public bool GetGameTutorials() {
      if (PlayerPrefs.HasKey(GAME_TUTORIALS)) {
        return PlayerPrefs.GetInt(GAME_TUTORIALS) == 1;
      }
      return false;
    }

    public void SetPhotoMode(bool enabled) {
      PlayerPrefs.SetInt(PHOTO_MODE, enabled ? 1 : 0);
    }

    public bool GetPhotoMode() {
      if (PlayerPrefs.HasKey(PHOTO_MODE)) {
        return PlayerPrefs.GetInt(PHOTO_MODE) == 1;
      }
      return false;
    }

    public int GetFPSLimit() {
      return PlayerPrefs.GetInt(FPS_KEY, 60);
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

    public void SetQuality(QualityIndex qualityIndex) {
#if UNITY_EDITOR
      Debug.Log($"Will change quality index to {qualityIndex} in production.");
#else
      QualitySettings.SetQualityLevel((int)qualityIndex, true);
#endif
      PlayerPrefs.SetInt(QUALITY_KEY, (int)qualityIndex);
    }

    public QualityIndex GetQuality() {
      return (QualityIndex)PlayerPrefs.GetInt(QUALITY_KEY, (int)QualityIndex.MEDIUM);
    }

    public int GetDisplayMode() {
      return PlayerPrefs.GetInt(DISPLAY_MODE_KEY, 1);
    }

    public void SetDisplayMode(int mode) {
      PlayerPrefs.SetInt(DISPLAY_MODE_KEY, mode);
    }

    public string GetGraphicsDeviceName() {
      return SystemInfo.graphicsDeviceName;
    }

    public void SaveAll() {
      PlayerPrefs.Save();
    }

    void OnDisable() {
      PlayerPrefs.Save();
    }
  }
}
