using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

namespace Solace {
  public class LanguageMenuController : MenuController {
    public TMP_Dropdown languageDropdown;
    private Coroutine optionsCoroutine;

    void Start() {
      languageDropdown.ClearOptions();
      if (optionsCoroutine != null) {
        StopCoroutine(optionsCoroutine);
      }
      optionsCoroutine = StartCoroutine(LoadLanguagesAsync());
      LocalizationSettings.SelectedLocaleChanged += OnLocaleChanged;
    }

    private void OnLocaleChanged(Locale locale) {
      int index = LocalizationSettings.AvailableLocales.Locales.FindIndex(a => a.Identifier.Code == locale.Identifier.Code);
      if (index >= 0) {
        languageDropdown.value = index;
      }
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
        languageDropdown.options.Add(new(nativeName));
      }
      languageDropdown.value = selectedIndex;
      languageDropdown.onValueChanged.AddListener(OnLanguageChanged);
    }

    private void OnLanguageChanged(int index) {
      Locale selectedLocale = LocalizationSettings.AvailableLocales.Locales[index];
      LocalizationSettings.SelectedLocale = selectedLocale;
    }

    private void OnDisable() {
      LocalizationSettings.SelectedLocaleChanged -= OnLocaleChanged;
      if (optionsCoroutine != null) {
        StopCoroutine(optionsCoroutine);
      }
    }
  }
}
