using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Solace {
  public class CinematicButton : MonoBehaviour {
    private float indicatorTimer = 0f;
    private const float maxIndicatorTimer = 1f;

    private Image radialIndicatorUI;

    private bool inCinematicMode;
    private bool isPressing;
    private bool isReleasing;
    private bool hasTriggered;

    public delegate void CinematicAction(bool shouldShow);
    public static event CinematicAction DidUseCinematicAction;

    private void OnCinematicMode(bool pressing) {
      if (pressing) {
        isPressing = true;
        isReleasing = false;
      }
      else {
        isPressing = false;
        isReleasing = true;
      }
    }

    private void OnEnable() {
      InputManager.DidUseCinematicMode += OnCinematicMode;
    }

    private void OnDisable() {
      InputManager.DidUseCinematicMode -= OnCinematicMode;
    }

    private void Start() {
      radialIndicatorUI = GetComponent<Image>();
    }

    private void Update() {
      if (!inCinematicMode && isPressing) {
        hasTriggered = true;
        radialIndicatorUI.enabled = true;
        if (indicatorTimer <= maxIndicatorTimer) {
          indicatorTimer += Time.deltaTime;
        }
        radialIndicatorUI.fillAmount = indicatorTimer;
        // If the indicator has fully filled up, hide the indicator and start cinematic mode
        if (indicatorTimer >= maxIndicatorTimer) {
          indicatorTimer = 0f;
          inCinematicMode = true;
          radialIndicatorUI.enabled = false;
          DidUseCinematicAction?.Invoke(true);
        }
      } else if (isReleasing) {
        // If the indicator has stopped midway, reverse the transition and stop cinematic mode.
        if (hasTriggered) {
          if (indicatorTimer >= 0f) {
            indicatorTimer -= Time.deltaTime;
          }
          radialIndicatorUI.fillAmount = indicatorTimer;
          if (indicatorTimer <= 0f) {
            radialIndicatorUI.enabled = false;
            hasTriggered = false;
            DidUseCinematicAction?.Invoke(false);
          }
        }
      }
    }
  }
}
