using UnityEditor;
using UnityEngine;

namespace Solace {
  [ExecuteInEditMode]
  [RequireComponent(typeof(HorizontalSelector))]
  public class HorizontalSelectorLive : MonoBehaviour {
    // Start is called before the first frame update
    void Start() {
      GetComponent<HorizontalSelector>().UpdateSelectionText();
    }

    // Update is called once per frame
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
