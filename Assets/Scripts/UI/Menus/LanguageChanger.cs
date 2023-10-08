using System.Collections;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.SmartFormat.Core.Parsing;

namespace Solace {
  [RequireComponent(typeof(LastUI.HorizontalSelector))]
  public class LanguageChanger : MonoBehaviour {
    private LastUI.HorizontalSelector selector;
    private Coroutine optionsCoroutine;

    private void Awake() {
      selector = GetComponent<LastUI.HorizontalSelector>();
    }

    private void OnEnable() {
      if (optionsCoroutine != null) {
        StopCoroutine(optionsCoroutine);
      }
      selector.data.Clear();
      optionsCoroutine = StartCoroutine(LoadLanguagesAsync());
      LocalizationSettings.SelectedLocaleChanged += OnLocaleChanged;
    }

    private void OnLanguageChanged(int index) {
      Locale selectedLocale = LocalizationSettings.AvailableLocales.Locales[index];
      LocalizationSettings.SelectedLocale = selectedLocale;
    }

    private IEnumerator LoadLanguagesAsync() {
      yield return LocalizationSettings.InitializationOperation;
      string playerLocale = SettingsManager.instance.GetLanguage();
      int selectedIndex = 0;
      for (int i = 0; i < LocalizationSettings.AvailableLocales.Locales.Count; ++i) {
        var locale = LocalizationSettings.AvailableLocales.Locales[i];
        string nativeName = locale.Identifier.CultureInfo.NativeName;
        if (LocalizationSettings.SelectedLocale == locale) {
          selectedIndex = i;
        }
        selector.data.Add(new(nativeName));
      }
      selector.index = selectedIndex;
      selector.OnValueChanged += OnLanguageChanged;
    }

    private void OnLocaleChanged(Locale locale) {
      int index = LocalizationSettings.AvailableLocales.Locales.FindIndex(a => a.Identifier.Code == locale.Identifier.Code);
      if (index >= 0) {
        selector.index = index;
      }
    }

    private void OnDisable() {
      LocalizationSettings.SelectedLocaleChanged -= OnLocaleChanged;
      if (optionsCoroutine != null) {
        StopCoroutine(optionsCoroutine);
      }
    }
  }
}
