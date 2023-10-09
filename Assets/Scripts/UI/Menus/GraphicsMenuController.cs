using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using LastUI;

namespace Solace {
  public class GraphicsMenuController : MonoBehaviour {
    public HorizontalSelector graphicalQualitySelector;
    public HorizontalSelector textureQualitySelector;
    public HorizontalSelector anisoFilteringSelector;
    public HorizontalSelector shadowQualitySelector;
    public HorizontalSelector ambientOcclusionSelector;
    public HorizontalSelector depthOfFieldSelector;
    public Toggle bloomToggle;
    public Toggle motionBlurToggle;
    public Toggle ssrToggle;
    public Toggle lensFlaresToggle;

    // Quality
    private List<string> qualityOptions = new() {
      "Very Low", "Low", "Medium", "High", "Very High", "Ultra"
    };

    private void OnQualityChanged(int qualityIndex) {
      QualityIndex preferedIndex = (QualityIndex)qualityIndex;
      SettingsManager.instance.SetQuality(preferedIndex);
    }

    void Start() {
      graphicalQualitySelector.index = (int)SettingsManager.instance.GetQuality();
    }

    private void OnEnable() {
      graphicalQualitySelector.OnValueChanged += OnQualityChanged;
    }

    private void OnDisable() {
      graphicalQualitySelector.OnValueChanged -= OnQualityChanged;
    }
  }
}
