using UnityEngine;
using UnityEngine.UI;

namespace Solace {
  public class MainMenuController : MenuController {
    public Button newGameButton;
    public Button loadGameButton;
    public Button settingsButton;
    public Button exitButton;

    void OnEnable() {
      newGameButton.onClick.AddListener(OnNewGameClick);
      loadGameButton.onClick.AddListener(OnLoadGameClick);
      settingsButton.onClick.AddListener(OnSettingsClick);
      exitButton.onClick.AddListener(OnExitClick);
    }

    private void OnExitClick() {
#if UNITY_STANDALONE
      Application.Quit();
#endif
#if UNITY_EDITOR
      UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    private void OnSettingsClick() {
      MenuCanvasManager.instance.SwitchMenu(MenuType.SETTINGS_MENU);
    }

    private void OnLoadGameClick() {
      Debug.Log("Will Load From Saved Games");
    }

    private void OnNewGameClick() {
      LevelManager instance = LevelManager.instance;
      if (instance != null) {
        instance.LoadScene(LevelManager.PROTOTYPE_LEVEL);
      }
      else {
        Debug.LogWarning("LevelManager is not added in the scene.");
      }
    }

    private void OnDisable() {
      newGameButton.onClick.RemoveAllListeners();
      loadGameButton.onClick.RemoveAllListeners();
      settingsButton.onClick.RemoveAllListeners();
      exitButton.onClick.RemoveAllListeners();
    }
  }
}
