using System.Linq;
using TMPro;

namespace Solace {
  public class LanguageMenuController : MenuController {
    public TMP_Dropdown languageDropdown;

    void Start() {
      var languages = SettingsManager.instance.GetLanguages();
      languageDropdown.ClearOptions();
      languageDropdown.AddOptions(languages.Values.ToList());
    }

    void Update() {
    
    }
  }
}
