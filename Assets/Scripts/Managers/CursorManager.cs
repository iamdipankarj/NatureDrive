using UnityEngine;

namespace Solace {
  public class CursorManager : MonoBehaviour {
    public static void LockCursor() {
      Cursor.lockState = CursorLockMode.Locked;
      Cursor.visible = false;
    }

    public static void UnlockCursor() {
      Cursor.lockState = CursorLockMode.None;
      Cursor.visible = true;
    }

    // Update is called once per frame
    void Update() {
    
    }
  }
}
