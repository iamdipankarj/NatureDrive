using System;
using UnityEngine;
using UnityEngine.UI;

namespace Solace {
  public class CinematicButton : MonoBehaviour {
    private float indicatorTimer = 0f;
    private const float maxIndicatorTimer = 1f;
    public readonly static float transitionDuration = 1f;

    private Image radialIndicatorUI;

    private bool inCinematicMode;
    private bool isPressing;
    private bool isReleasing;
    private bool hasTriggered;

    public delegate void CinematicEnterAction();
    public static event CinematicEnterAction DidEnterCinematicAction;

    public delegate void CinematicBarTranslateAction(float delta);
    public static event CinematicBarTranslateAction DidTranslateCinematicBars;

    public delegate void CinematicExitAction();
    public static event CinematicExitAction DidExitCinematicAction;

    private void OnCinematicMode(bool pressing) {
      if (inCinematicMode) {
        if (pressing) {
          isPressing = false;
          isReleasing = false;
          inCinematicMode = false;
          DidExitCinematicAction?.Invoke();
        }
      } else {
        if (pressing) {
          isPressing = true;
          isReleasing = false;
        } else {
          isPressing = false;
          isReleasing = true;
        }
      }
    }

    private void Start() {
      radialIndicatorUI = GetComponent<Image>();
    }

    private void Update() {
      if (isPressing) {
        // Enable the UI
        radialIndicatorUI.enabled = true;

        // Record the event to avoid unnecesary state checks
        hasTriggered = true;

        // Incerement the timer
        indicatorTimer += Time.deltaTime * transitionDuration;

        // if the timer is within limits
        if (indicatorTimer <= maxIndicatorTimer) {
          radialIndicatorUI.fillAmount = indicatorTimer;
          DidTranslateCinematicBars?.Invoke(indicatorTimer);
        }

        // If the timer is out of bounds, reset the timer and Invoke cinematic enter mode
        if (indicatorTimer > maxIndicatorTimer) {
          indicatorTimer = maxIndicatorTimer;

          // Hide the UI
          radialIndicatorUI.enabled = false;

          // Do not invoke again if already in cinematic mode
          if (!inCinematicMode) {
            inCinematicMode = true;
            DidEnterCinematicAction?.Invoke();
          }
        }
      }
      else if (isReleasing) {
        if (hasTriggered) {
          if (indicatorTimer >= 0f) {
            indicatorTimer -= Time.deltaTime * transitionDuration;
            radialIndicatorUI.fillAmount = indicatorTimer;
            DidTranslateCinematicBars?.Invoke(indicatorTimer);
          }
          if (indicatorTimer <= 0f) {
            hasTriggered = false;
          }
        }
      }
    }

    private void OnEnable() {
      InputManager.DidUseCinematicMode += OnCinematicMode;
    }

    private void OnDisable() {
      InputManager.DidUseCinematicMode -= OnCinematicMode;
    }
  }
}
