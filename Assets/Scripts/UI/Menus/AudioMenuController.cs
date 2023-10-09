using System;
using UnityEngine;
using UnityEngine.UI;

namespace Solace {
  public class AudioMenuController : MonoBehaviour {
    public Slider musicSlider;
    public Slider sfxSlider;
    public Slider ambientSlider;

    void Start() {
      musicSlider.value = SettingsManager.instance.GetMusicVolume();
      sfxSlider.value = SettingsManager.instance.GetSFXVolume();
      ambientSlider.value = SettingsManager.instance.GetAmbientVolume();
    }

    private void OnEnable() {
      musicSlider.onValueChanged.AddListener(OnMusicValueChanged);
      sfxSlider.onValueChanged.AddListener(OnSFXValueChanged);
      ambientSlider.onValueChanged.AddListener(OnAmbientValueChanged);
    }

    private void OnAmbientValueChanged(float value) {
      SettingsManager.instance.SetAmbientVolume((int)value);
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
      ambientSlider.onValueChanged.RemoveListener(OnAmbientValueChanged);
    }
  }
}
