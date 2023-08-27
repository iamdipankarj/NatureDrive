using System;
using UnityEngine.UI;

namespace Solace {
  public class AudioMenuController : MenuController {
    public Slider musicSlider;
    public Slider sfxSlider;

    void Start() {
      musicSlider.value = SettingsManager.instance.GetMusicVolume();
      sfxSlider.value = SettingsManager.instance.GetSFXVolume();
    }

    private void OnEnable() {
      musicSlider.onValueChanged.AddListener(OnMusicValueChanged);
      sfxSlider.onValueChanged.AddListener(OnSFXValueChanged);
    }

    private void OnSFXValueChanged(float value) {
      SettingsManager.instance.SetSFXVolume((int)value);
    }

    private void OnMusicValueChanged(float value) {
      SettingsManager.instance.SetMusicVolume((int)value);
    }

    private void OnDisable() {
      musicSlider.onValueChanged.RemoveListener(OnMusicValueChanged);
      sfxSlider.onValueChanged.RemoveListener(OnSFXValueChanged);
    }
  }
}
