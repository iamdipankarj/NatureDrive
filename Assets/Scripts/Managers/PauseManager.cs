using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Rewired;

namespace Solace {
  public enum FadeType {
    IN,
    OUT
  }
  public class PauseManager : MonoBehaviour {
    public static PauseManager instance;
    private bool isPaused = false;
    private Player player;

    public delegate void PauseEventHandler(bool isPaused);
    public static event PauseEventHandler OnTogglePause;

    public Button resumeButton;
    public Button mainMenuButton;
    public Button restartLevelButton;

    public Image panel;
    public RectTransform content;
    private CanvasRenderer panelRenderer;
    private Coroutine fadeCoroutine;
    private const float fadeSpeed = 8f;

    private void LockCursor() {
      Cursor.lockState = CursorLockMode.Locked;
      Cursor.visible = false;
    }

    private void UnlockCursor() {
      Cursor.lockState = CursorLockMode.None;
      Cursor.visible = true;
    }

    private void TogglePause() {
      if (isPaused) {
        ResumeGame();
      } else {
        PauseGame();
      }
    }

    private void PauseGame() {
      UnlockCursor();
      content.gameObject.SetActive(true);
      isPaused = true;
      PauseTime();
      OnTogglePause?.Invoke(true);
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
      UnlockCursor();
      content.gameObject.SetActive(false);
      isPaused = false;
      UnPauseTime();
      OnTogglePause?.Invoke(false);
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
      LevelManager.instance.LoadScene(LevelManager.MAIN_MENU_LEVEL);
    }

    private void OnEnable() {
      resumeButton.onClick.AddListener(ResumeGame);
      mainMenuButton.onClick.AddListener(OnMainMenuClick);
      restartLevelButton.onClick.AddListener(OnRestartLevelClick);
    }

    private void OnRestartLevelClick() {
      UnPauseTime();
      LevelManager.instance.RestartLevel();
    }

    private void OnDisable() {
      resumeButton.onClick.RemoveListener(ResumeGame);
      mainMenuButton.onClick.RemoveListener(OnMainMenuClick);
      restartLevelButton.onClick.RemoveListener(OnRestartLevelClick);
      if (fadeCoroutine != null) {
        StopCoroutine(fadeCoroutine);
      }
    }

    private void Update() {
      if (player.GetButtonDown(RewiredUtils.Pause)) {
        TogglePause();
      }
    }

    private void Start() {
      LockCursor();
      panelRenderer = panel.GetComponent<CanvasRenderer>();
      panel.gameObject.SetActive(true);
      content.gameObject.SetActive(false);
      panelRenderer.SetAlpha(0f);
    }

    private void Awake() {
      player = ReInput.players.GetPlayer(0);
      if (instance == null) {
        instance = this;
      }
      else {
        Destroy(gameObject);
      }
    }
  }
}
