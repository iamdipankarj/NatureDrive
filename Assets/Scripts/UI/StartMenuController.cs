using System;
using UnityEngine;
using UnityEngine.UI;

namespace Solace {
  public class StartMenuController : MonoBehaviour {
    public Button newGameButton;
    public Button SettingsButton;

    void Start() {
      newGameButton.onClick.AddListener(OnNewGameClick);
    }

    private void OnNewGameClick() {
      LevelManager instance = LevelManager.instance;
      if (instance != null) {
        instance.LoadScene(LevelManager.PROTOTYPE_LEVEL);
      } else {
        Debug.LogWarning("LevelManager is not added in the scene.");
      }
    }

    // Update is called once per frame
    void Update() {
    
    }
  }
}
