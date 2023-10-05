using UnityEngine;
using UnityEngine.UI;

namespace LastUI {
  public class StateController : MonoBehaviour {
    [Header("Canvas Type")]
    public CanvasType canvasType;

    [Header("Canvas Settings")]
    //public bool canGoPreviousCanvas;
    public CanvasType previousCanvas;

    [Header("UI Settings")]
    public Button StartSelectable;
    StateManager stateManager;

    private void OnEnable() {
      stateManager = StateManager.instance;
    }
  }
}