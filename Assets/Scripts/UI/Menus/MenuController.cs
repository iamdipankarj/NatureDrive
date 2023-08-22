using UnityEngine;

namespace Solace {
  public enum MenuType {
    MAIN_MENU,
    SETTINGS_MENU,
    INPUT_MENU,
    GAME_OPTIONS_MENU,
    AUDIO_OPTIONS_MENU,
    LANGUAGE_OPTIONS_MENU,
    GRAPHICS_OPTIONS_MENU
  }

  public class MenuController : MonoBehaviour {
    public MenuType menuType;
  }
}
