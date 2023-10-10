using UnityEngine;
using UnityEngine.UI;
using LastUI;
using System;

namespace Solace {
  public class GraphicsMenuController : MonoBehaviour {
    public HorizontalSelector graphicalQualitySelector;
    public HorizontalSelector anisoFilteringSelector;
    public HorizontalSelector shadowQualitySelector;
    public HorizontalSelector ambientOcclusionSelector;
    public HorizontalSelector depthOfFieldSelector;
    public Toggle bloomToggle;
    public Toggle motionBlurToggle;
    public Toggle ssrToggle;
    public Toggle lensFlaresToggle;
    private const int OFF_STATE = 0;
    private const int ON_STATE = 1;

    private void OnQualityChanged(int qualityIndex) {
      QualityIndex preferedIndex = (QualityIndex)qualityIndex;
      SettingsManager.instance.SetQuality(preferedIndex);
      QualitySettings.anisotropicFiltering = AnisotropicFiltering.Enable;
    }

    private void OnShadowQualityChanged(int index) {
      if (index == 0) {
        QualitySettings.shadows = ShadowQuality.Disable;
      } else if (index == 1) {
        QualitySettings.shadows = ShadowQuality.HardOnly;
      } else if (index == 2) {
        QualitySettings.shadows = ShadowQuality.All;
      }
      SettingsManager.instance.SetShadowQuality(index);
    }

    private void OnAnisoFilteringChanged(int index) {
      if (index == OFF_STATE) {
        QualitySettings.anisotropicFiltering = AnisotropicFiltering.Disable;
      } else if (index == ON_STATE) {
        QualitySettings.anisotropicFiltering = AnisotropicFiltering.ForceEnable;
      }
      SettingsManager.instance.SetAnisoFilteringEnabled(index == ON_STATE);
    }

    private void OnAmbientValueChanged(int index) {
      SettingsManager.instance.SetAnisoFilteringEnabled(index == ON_STATE);
    }

    private void OnDofValueChanged(int index) {
      SettingsManager.instance.SetDepthOfField(index == ON_STATE);
    }

    private void OnBloomChanged(bool value) {
      SettingsManager.instance.SetBloomEnabled(value);
    }

    private void OnMotionBlurChanged(bool value) {
      SettingsManager.instance.SetMotionBlurEnabled(value);
    }

    private void OnSsrChanged(bool value) {
      SettingsManager.instance.SetScreenSpaceReflectionsEnabled(value);
    }

    private void OnLensFlaresChanged(bool value) {
      SettingsManager.instance.SetLensFlaresEnabled(value);
    }

    void Start() {
      graphicalQualitySelector.index = (int)SettingsManager.instance.GetQuality();
      anisoFilteringSelector.index = SettingsManager.instance.GetAnisoFilteringEnabled() ? ON_STATE : OFF_STATE;
      shadowQualitySelector.index = SettingsManager.instance.GetShadowQuality();
      ambientOcclusionSelector.index = SettingsManager.instance.GetAmbientOcclusionEnabled() ? ON_STATE : OFF_STATE;
      depthOfFieldSelector.index = SettingsManager.instance.GetDepthOfFieldEnabled() ? ON_STATE : OFF_STATE;
      bloomToggle.isOn = SettingsManager.instance.GetBloomEnabled();
      motionBlurToggle.isOn = SettingsManager.instance.GetMotionBlurEnabled();
      ssrToggle.isOn = SettingsManager.instance.GetScreenSpaceReflectionsEnabled();
      lensFlaresToggle.isOn = SettingsManager.instance.GetLensFlaresEnabled();
    }

    private void OnEnable() {
      graphicalQualitySelector.OnValueChanged += OnQualityChanged;
      anisoFilteringSelector.OnValueChanged += OnAnisoFilteringChanged;
      shadowQualitySelector.OnValueChanged += OnShadowQualityChanged;
      ambientOcclusionSelector.OnValueChanged += OnAmbientValueChanged;
      depthOfFieldSelector.OnValueChanged += OnDofValueChanged;
      bloomToggle.onValueChanged.AddListener(OnBloomChanged);
      motionBlurToggle.onValueChanged.AddListener(OnMotionBlurChanged);
      ssrToggle.onValueChanged.AddListener(OnSsrChanged);
      lensFlaresToggle.onValueChanged.AddListener(OnLensFlaresChanged);
    }

    private void OnDisable() {
      graphicalQualitySelector.OnValueChanged -= OnQualityChanged;
      anisoFilteringSelector.OnValueChanged -= OnAnisoFilteringChanged;
      shadowQualitySelector.OnValueChanged -= OnShadowQualityChanged;
      ambientOcclusionSelector.OnValueChanged -= OnAmbientValueChanged;
      depthOfFieldSelector.OnValueChanged -= OnDofValueChanged;
      bloomToggle.onValueChanged.RemoveListener(OnBloomChanged);
      motionBlurToggle.onValueChanged.RemoveListener(OnMotionBlurChanged);
      ssrToggle.onValueChanged.RemoveListener(OnSsrChanged);
      lensFlaresToggle.onValueChanged.RemoveListener(OnLensFlaresChanged);
    }
  }
}
