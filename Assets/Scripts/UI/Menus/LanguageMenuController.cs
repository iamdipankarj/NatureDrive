using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

namespace Solace {
  public class LanguageOptionData : TMP_Dropdown.OptionData {
    [SerializeField]
    private string m_Code;
    public string code { get { return m_Code; } set { m_Code = value; } }
  }

  public class LanguageMenuController : MenuController {
    public TMP_Dropdown languageDropdown;

    void Start() {
      var languages = SettingsManager.instance.GetLanguages();
      languageDropdown.ClearOptions();
      languageDropdown.AddOptions(languages.Keys.ToList());
      languageDropdown.onValueChanged.AddListener(OnLanguageChanged);
    }

    private void OnLanguageChanged(int index) {
      if (index < languageDropdown.options.Count) {
        string localeDisplayName = languageDropdown.options[index].text;
        string locale = SettingsManager.instance.GetLanguages()[localeDisplayName];
        LocalizationSettings.SelectedLocale = Locale.CreateLocale(locale);
      } else {
        Debug.LogWarning("Language dropdown selection index out of range");
      }
    }

    void Update() {
    
    }
  }
}
