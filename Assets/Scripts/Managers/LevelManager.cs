using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Solace {
  public class LevelManager : MonoBehaviour {
    public static readonly string PROTOTYPE_LEVEL = "Prototype";
    public static readonly string MAIN_MENU_LEVEL = "MainMenu";

    public static LevelManager instance;
    public Canvas loaderCanvas;
    public Slider loadingSlider;

    private void Awake() {
      if (instance == null) {
        instance = this;
      }
      else {
        Destroy(gameObject);
      }
      DontDestroyOnLoad(gameObject);
    }

    public void RestartLevel() {
      SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadScene(string sceneName) {
      loaderCanvas.gameObject.SetActive(true);
      StartCoroutine(LoadSceneAsync(sceneName));
    }

    IEnumerator LoadSceneAsync(string sceneName) {
      AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
      while (!operation.isDone) {
        float progress = Mathf.Clamp01(operation.progress / 0.9f) * 100;
        loadingSlider.value = progress;
        yield return null;
      }
      loaderCanvas.gameObject.SetActive(false);
    }
  }
}
