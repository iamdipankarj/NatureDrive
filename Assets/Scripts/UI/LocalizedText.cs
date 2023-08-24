using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.Settings;

namespace Solace {
  [RequireComponent(typeof(LocalizeStringEvent))]
  [RequireComponent(typeof(TextMeshProUGUI))]
  public class LocalizedText : MonoBehaviour {
    private TextMeshProUGUI textComponent;

    private void Start() {
      textComponent = GetComponent<TextMeshProUGUI>();
      if (TryGetComponent<LocalizeStringEvent>(out var comp)) {
        comp.OnUpdateString.AddListener(OnSelectorLocaleUpdate);
      }
    }

    private void OnSelectorLocaleUpdate(string localeText) {
#if UNITY_EDITOR
      Debug.Log($"Changed Locale to: {LocalizationSettings.SelectedLocale.LocaleName}");
#endif
      textComponent.text = localeText;
    }

    private void OnDisable() {
      if (TryGetComponent<LocalizeStringEvent>(out var comp)) {
        comp.OnUpdateString.RemoveListener(OnSelectorLocaleUpdate);
      }
    }
  }
}
