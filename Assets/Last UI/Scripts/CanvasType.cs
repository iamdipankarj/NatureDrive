using UnityEngine;

namespace LastUI {
  [CreateAssetMenu(menuName = "Last UI/Canvas Editor/New Canvas")]
  public class CanvasType : ScriptableObject {
    public string title;
    public bool canGoPreviousCanvas;
  }
}
