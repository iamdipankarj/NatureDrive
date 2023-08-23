using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Solace {
  public enum FadeType {
    IN,
    OUT
  }
  public class PauseManager : MonoBehaviour {
    public static PauseManager instance;
    [HideInInspector]
    public bool isPaused = false;

    public Button resumeButton;
    public Button mainMenuButton;

    public Image panel;
    public RectTransform content;
    private CanvasRenderer panelRenderer;
    private Coroutine fadeCoroutine;
    private const float fadeSpeed = 8f;

    private void OnPause() {
      if (isPaused) {
        ResumeGame();
      } else {
        PauseGame();
      }
    }

    private void PauseGame() {
      InputSystem.PauseHaptics();
      content.gameObject.SetActive(true);
      CursorManager.UnlockCursor();
      isPaused = true;
      PauseTime();
      if (fadeCoroutine != null) {
        StopCoroutine(fadeCoroutine);
      }
      fadeCoroutine = StartCoroutine(Fade(FadeType.IN));
    }

    private void PauseTime() {
      Time.timeScale = 0f;
    }

    private void UnPauseTime() {
      Time.timeScale = 1f;
    }

    private void ResumeGame() {
      InputSystem.ResumeHaptics();
      content.gameObject.SetActive(false);
      CursorManager.LockCursor();
      isPaused = false;
      UnPauseTime();
      if (fadeCoroutine != null) {
        StopCoroutine(fadeCoroutine);
      }
      fadeCoroutine = StartCoroutine(Fade(FadeType.OUT));
    }

    private IEnumerator Fade(FadeType fadeType) {
      float time = 0;
      float startAlpha = panelRenderer.GetAlpha();
      float endAlpha = fadeType == FadeType.IN ? 1f : 0f;
      while (time < 1) {
        float alpha = Mathf.Lerp(startAlpha, endAlpha, time);
        panelRenderer.SetAlpha(alpha);
        yield return null;
        time += Time.unscaledDeltaTime * fadeSpeed;
      }
    }

    private void OnMainMenuClick() {
      UnPauseTime();
      CursorManager.UnlockCursor();
      LevelManager.instance.LoadScene(LevelManager.MAIN_MENU_LEVEL);
    }

    private void OnEnable() {
      InputManager.DidPause += OnPause;
      resumeButton.onClick.AddListener(ResumeGame);
      mainMenuButton.onClick.AddListener(OnMainMenuClick);
    }

    private void OnDisable() {
      InputManager.DidPause -= OnPause;
      resumeButton.onClick.RemoveListener(ResumeGame);
      mainMenuButton.onClick.RemoveListener(OnMainMenuClick);
      if (fadeCoroutine != null) {
        StopCoroutine(fadeCoroutine);
      }
    }

    private void Start() {
      panelRenderer = panel.GetComponent<CanvasRenderer>();
      panel.gameObject.SetActive(true);
      content.gameObject.SetActive(false);
      panelRenderer.SetAlpha(0f);
    }

    private void Awake() {
      if (instance == null) {
        instance = this;
      }
      else {
        Destroy(gameObject);
      }
    }
  }
}
