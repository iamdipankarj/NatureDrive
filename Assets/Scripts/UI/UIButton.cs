using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Solace {
  public class UIButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    [SerializeField]
    private TMP_Text text;
    private Color defaultColor;

    void Start() {
      if (text == null) {
        Debug.LogWarning("Text component is not attached.");
      } else {
        defaultColor = text.color;
      }
    }

    public void OnPointerEnter(PointerEventData eventData) {
      text.color = ColorManager.accentColor;
    }

    public void OnPointerExit(PointerEventData eventData) {
      text.color = defaultColor;
    }

    void Update() {
    
    }
  }
}
