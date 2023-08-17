using System.Collections;
using UnityEngine;
using TMPro;

namespace Solace {
  public class NotificationManager : MonoBehaviour {
    public static NotificationManager instance;
    public TMP_Text canvasText;
    private const float destroyDelay = 3.0f;

    private void Awake() {
      if (instance == null) {
        instance = this;
      }
      else {
        Destroy(gameObject);
      }
      DontDestroyOnLoad(gameObject);
    }

    public void Notify(string text) {
      StartCoroutine(NotifyAsync(text));
    }

    public void ShowHint(string text) {
      canvasText.SetText(text);
      canvasText.gameObject.SetActive(true);
    }

    public void HideHint() {
      canvasText.gameObject.SetActive(false);
    }

    IEnumerator NotifyAsync(string text) {
      canvasText.gameObject.SetActive(true);
      canvasText.SetText(text);
      yield return new WaitForSeconds(destroyDelay);
      canvasText.gameObject.SetActive(false);
    }
  }
}
