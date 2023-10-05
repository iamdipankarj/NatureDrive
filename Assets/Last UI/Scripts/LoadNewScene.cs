using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace LastUI {
  public class LoadNewScene : MonoBehaviour {

    [HideInInspector] public GameObject LoadingPanel;
    [HideInInspector] public Image LoadingBar;

    public void LoadANewScene(string scene) {
      StartCoroutine(LoadSceneAsync(scene));
    }

    IEnumerator LoadSceneAsync(string scene) {
      AsyncOperation operation = SceneManager.LoadSceneAsync(scene);
      LoadingPanel.SetActive(true);

      while (operation.isDone) {
        float progressValue = Mathf.Clamp01(operation.progress / 0.9f);

        LoadingBar.fillAmount = progressValue;

        yield return null;
      }
    }
  }
}