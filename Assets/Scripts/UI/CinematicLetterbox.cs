using System;
using System.Collections;
using UnityEngine;

namespace Solace {
  public class CinematicLetterbox : MonoBehaviour {
    [SerializeField]
    private RectTransform topBar;
    [SerializeField]
    private RectTransform bottomBar;

    private float barHeight;
    private float positionDelta = 0f;

    private void OnTranslateCinematicBars(float delta) {
      positionDelta = delta;
    }

    private void OnExitCinematicMode() {
      Debug.Log("Exit Cinematic Mode");
    }

    private void OnEnterCinematicMode() {
      Debug.Log("Enter Cinematic Mode");
    }

    void Start() {
      if (topBar == null || bottomBar == null) {
        Debug.LogWarning("Top and Bottom Bar not assigned");
      } else {
        barHeight = topBar.rect.height;
        CinematicButton.DidTranslateCinematicBars += OnTranslateCinematicBars;
        CinematicButton.DidEnterCinematicAction += OnEnterCinematicMode;
        CinematicButton.DidExitCinematicAction += OnExitCinematicMode;
      }
    }

    private void OnDisable() {
      CinematicButton.DidTranslateCinematicBars -= OnTranslateCinematicBars;
      CinematicButton.DidEnterCinematicAction -= OnEnterCinematicMode;
      CinematicButton.DidExitCinematicAction -= OnExitCinematicMode;
    }

    void Update() {
      if (topBar != null && bottomBar != null) {
        topBar.anchoredPosition = new Vector2(0f , -barHeight * positionDelta);
        bottomBar.anchoredPosition = new Vector2(0f, barHeight * positionDelta);
      }
    }
  }
}
