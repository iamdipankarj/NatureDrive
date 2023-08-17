using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Solace {
  public class HorizontalSelectedItem : MonoBehaviour, ISelectHandler, IDeselectHandler {
    private TextMeshProUGUI mesh;

    private void Start() {
      mesh = GetComponent<TextMeshProUGUI>();
    }

    public void OnSelect(BaseEventData eventData) {
      mesh.color = ColorManager.accentColor;
    }

    public void OnDeselect(BaseEventData eventData) {
      mesh.color = ColorManager.defaultColor;
    }
  }
}
