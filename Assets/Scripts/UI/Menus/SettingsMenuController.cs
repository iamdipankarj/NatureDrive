using System;
using UnityEngine.UI;

namespace Solace {
  public class SettingsMenuController : MenuController {
    public Button gameOptionsButton;
    public Button inputOptionsButton;
    public Button graphicsOptionsButton;
    public Button audioOptionsButton;
    public Button languagOptionsButton;

    void OnEnable() {
      gameOptionsButton.onClick.AddListener(OnGameOptionsClick);
      inputOptionsButton.onClick.AddListener(OnInputOptionsClick);
      graphicsOptionsButton.onClick.AddListener(OnGraphicsOptionsClick);
      audioOptionsButton.onClick.AddListener(OnAudioOptionsClick);
      languagOptionsButton.onClick.AddListener(OnLanguageOptionsClick);
    }

    private void OnBackButtonClick() {
      MenuCanvasManager.instance.SwitchMenu(MenuType.MAIN_MENU);
    }

    private void OnGameOptionsClick() {
      MenuCanvasManager.instance.SwitchMenu(MenuType.GAME_OPTIONS_MENU);
    }

    private void OnInputOptionsClick() {
      MenuCanvasManager.instance.SwitchMenu(MenuType.INPUT_MENU);
    }

    private void OnGraphicsOptionsClick() {
      MenuCanvasManager.instance.SwitchMenu(MenuType.GRAPHICS_OPTIONS_MENU);
    }

    private void OnAudioOptionsClick() {
      MenuCanvasManager.instance.SwitchMenu(MenuType.AUDIO_OPTIONS_MENU);
    }

    private void OnLanguageOptionsClick() {
      MenuCanvasManager.instance.SwitchMenu(MenuType.LANGUAGE_OPTIONS_MENU);
    }

    private void OnDisable() {
      gameOptionsButton.onClick.RemoveAllListeners();
      inputOptionsButton.onClick.RemoveAllListeners();
      graphicsOptionsButton.onClick.RemoveAllListeners();
      audioOptionsButton.onClick.RemoveAllListeners();
      languagOptionsButton.onClick.RemoveAllListeners();
    }

    void Start() {
    }
  }
}
