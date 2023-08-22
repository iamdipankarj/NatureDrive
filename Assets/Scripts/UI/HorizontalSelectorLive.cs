using UnityEditor;
using UnityEngine;

namespace Solace {
  [ExecuteInEditMode]
  [RequireComponent(typeof(HorizontalSelector))]
  public class HorizontalSelectorLive : MonoBehaviour {
    void Start() {
      GetComponent<HorizontalSelector>().UpdateSelectionText();
    }

    void Update() {
#if UNITY_EDITOR
      if (EditorApplication.isPlaying) {
        return;
      }
      GetComponent<HorizontalSelector>().UpdateSelectionText();
#endif
    }
  }
}
