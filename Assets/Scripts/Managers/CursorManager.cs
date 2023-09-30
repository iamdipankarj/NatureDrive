using UnityEngine;
using Rewired;

namespace Solace {
  public class CursorManager : MonoBehaviour {
    // Rewired
    private Player player;
    private bool isPaused = false;

    private void Awake() {
      player = ReInput.players.GetPlayer(0);
    }

    private void Start() {
      LockCursor();
    }

    private void LockCursor() {
      Cursor.lockState = CursorLockMode.Locked;
      Cursor.visible = false;
      isPaused = false;
    }

    private void TogglePauseState() {
      Time.timeScale = isPaused ? 0f : 1f;
    }

    private void UnlockCursor() {
      Cursor.lockState = CursorLockMode.None;
      Cursor.visible = true;
      isPaused = true;
    }

    void Update() {
      if (player.GetButtonDown(RewiredUtils.Pause)) {
        isPaused = !isPaused;
        TogglePauseState();
      }
    }
  }
}
