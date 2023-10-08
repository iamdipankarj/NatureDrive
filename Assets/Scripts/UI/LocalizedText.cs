using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;

namespace Solace {
  [RequireComponent(typeof(LocalizeStringEvent))]
  [RequireComponent(typeof(TextMeshProUGUI))]
  public class LocalizedText : MonoBehaviour {
    private TextMeshProUGUI textComponent;

    private void Awake() {
      textComponent = GetComponent<TextMeshProUGUI>();
    }

    private void Start() {
      if (TryGetComponent<LocalizeStringEvent>(out var comp)) {
        comp.OnUpdateString.AddListener(OnSelectorLocaleUpdate);
      }
    }

    private void OnSelectorLocaleUpdate(string localeText) {
      textComponent.text = localeText;
    }

    private void OnDisable() {
      if (TryGetComponent<LocalizeStringEvent>(out var comp)) {
        comp.OnUpdateString.RemoveListener(OnSelectorLocaleUpdate);
      }
    }
  }
}
